using Microsoft.EntityFrameworkCore;
using RESTcup.Models;

namespace RepositoryTest
{
    public class RepositoryUnitTest
    {
        // use database or list based repository for testing
        private bool useDatabase = false;

        // use in-memory database or real database for testing
        private bool inMemoryDatabase = true;

        private ICupsRepository repo;

        public RepositoryUnitTest()
        {
            if (useDatabase)
            {
                var optionsBuilder = new DbContextOptionsBuilder<CupsDBContext>();
                // https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets

                CupsDBContext _dbContext;

                if (inMemoryDatabase)
                {
                    optionsBuilder.UseInMemoryDatabase("TestDb");
                    _dbContext = new(optionsBuilder.Options);

                    _dbContext.Database.EnsureDeleted();
                    _dbContext.Database.EnsureCreated();
                }
                else
                {
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=anbodatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30"; ;

                    // string connectionString = Secrets.ConnectionStringSimply;
                    // connection string structure
                    //   "Data Source=mssql7.unoeuro.com;Initial Catalog=FROM simply.com;Persist Security Info=True;User ID=FROM simply.com;Password=DB PASSWORD FROM simply.com;TrustServerCertificate=True"
                    optionsBuilder.UseSqlServer(connectionString);
                    _dbContext = new(optionsBuilder.Options);
                    _dbContext.Database.EnsureCreated();
                    // clean database table: delete all rows
                    _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Cups");
                }
                repo = new CupsRepositoryDatabase(_dbContext);
            }
            else
            {
                repo = new CupsRepositoryList();
            }
        }

        [Fact]
        public void Add_AssignsIdAndAddsToRepository()
        {
            Cup cup = new Cup { Color = "Yellow", Volume = 150 };

            Cup added = repo.AddCup(cup);

            Assert.Equal(1, added.Id);
            Cup? fetched = repo.GetCupById(1);
            Assert.NotNull(fetched);
            Assert.Equal("Yellow", fetched.Color);
            Assert.Equal(150, fetched.Volume);

            IEnumerable<Cup> all = repo.GetCups();
            Assert.Single(all);
            Assert.Equal(added, all.First());
        }

        //[Fact]
        // start with an empty repository (list or database table)
        public void Constructor_WithIncludeData_PopulatesRepository()
        {
            List<Cup> all = repo.GetCups().ToList();
            Assert.Equal(3, all.Count);

            Assert.Equal(1, all[0].Id);
            Assert.Equal("Red", all[0].Color);

            Assert.Equal(2, all[1].Id);
            Assert.Equal("Blue", all[1].Color);

            Assert.Equal(3, all[2].Id);
            Assert.Equal("Green", all[2].Color);
        }

        [Fact]
        public void Remove_RemovesExisting_ReturnsTrue()
        {
            Cup cup = new Cup() { Color = "Purple", Volume = 200 };
            Cup cup2 = new Cup() { Color = "Orange", Volume = 250 };

            repo.AddCup(cup);
            repo.AddCup(cup2);

            Cup? removed = repo.RemoveCup(1);

            Assert.NotNull(removed);
            Cup? fetched = repo.GetCupById(1);
            Assert.Null(fetched);
            IEnumerable<Cup> all = repo.GetCups();
            Assert.Single(all);
            Assert.DoesNotContain(all, (Cup c) => c.Id == 1);
        }

        [Fact]
        public void Remove_NonExisting_ReturnsFalse()
        {
            Cup? removed = repo.RemoveCup(999);

            Assert.Null(removed);
        }

        [Fact]
        public void Update_Existing_UpdatesProperties_ReturnsTrue()
        {
            Cup added = repo.AddCup(new Cup { Color = "Black", Volume = 100 });

            Cup updated = new Cup { Id = added.Id, Color = "White", Volume = 120 };
            Cup? result = repo.UpdateCup(updated);

            Assert.NotNull(result);
            Cup? fetched = repo.GetCupById(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal("White", fetched.Color);
            Assert.Equal(120, fetched.Volume);
        }

        [Fact]
        public void Update_NonExisting_ReturnsFalse()
        {
            Cup? result = repo.UpdateCup(new Cup { Id = 42, Color = "Pink", Volume = 50 });

            Assert.Null(result);
        }

        [Fact]
        public void GetById_NonExisting_ReturnsNull()
        {
            Cup? fetched = repo.GetCupById(12345);

            Assert.Null(fetched);
        }
    }
}
