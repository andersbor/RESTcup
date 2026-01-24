using RESTcup.Models;

namespace RepositoryTest
{
    public class UnitTest1
    {
        [Fact]
        public void Add_AssignsIdAndAddsToRepository()
        {
            CupsRepository repo = new CupsRepository();
            Cup cup = new Cup { Color = "Yellow", Volume = 150 };

            Cup added = repo.Add(cup);

            Assert.Equal(1, added.Id);
            Cup? fetched = repo.GetById(1);
            Assert.NotNull(fetched);
            Assert.Equal("Yellow", fetched.Color);
            Assert.Equal(150, fetched.Volume);

            IEnumerable<Cup> all = repo.GetAll();
            Assert.Single(all);
            Assert.Equal(added, all.First());
        }

        [Fact]
        public void Constructor_WithIncludeData_PopulatesRepository()
        {
            CupsRepository repo = new CupsRepository(includeData: true);

            List<Cup> all = repo.GetAll().ToList();
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
            CupsRepository repo = new CupsRepository(includeData: true);

            bool removed = repo.Remove(2);

            Assert.True(removed);
            Cup? fetched = repo.GetById(2);
            Assert.Null(fetched);
            IEnumerable<Cup> all = repo.GetAll();
            Assert.Equal(2, all.Count());
            Assert.DoesNotContain(all, (Cup c) => c.Id == 2);
        }

        [Fact]
        public void Remove_NonExisting_ReturnsFalse()
        {
            CupsRepository repo = new CupsRepository();

            bool removed = repo.Remove(999);

            Assert.False(removed);
        }

        [Fact]
        public void Update_Existing_UpdatesProperties_ReturnsTrue()
        {
            CupsRepository repo = new CupsRepository();
            Cup added = repo.Add(new Cup { Color = "Black", Volume = 100 });

            Cup updated = new Cup { Id = added.Id, Color = "White", Volume = 120 };
            bool result = repo.Update(updated);

            Assert.True(result);
            Cup? fetched = repo.GetById(added.Id);
            Assert.NotNull(fetched);
            Assert.Equal("White", fetched.Color);
            Assert.Equal(120, fetched.Volume);
        }

        [Fact]
        public void Update_NonExisting_ReturnsFalse()
        {
            CupsRepository repo = new CupsRepository();

            bool result = repo.Update(new Cup { Id = 42, Color = "Pink", Volume = 50 });

            Assert.False(result);
        }

        [Fact]
        public void GetById_NonExisting_ReturnsNull()
        {
            CupsRepository repo = new CupsRepository();

            Cup? fetched = repo.GetById(12345);

            Assert.Null(fetched);
        }
    }
}
