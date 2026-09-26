namespace RESTcup.Models
{
    public class CupsRepositoryDatabase : ICupsRepository
    {
        private CupsDBContext dbContext;

        public CupsRepositoryDatabase(CupsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }   
        public Cup AddCup(Cup cup)
        {
            dbContext.Add(cup);
            dbContext.SaveChanges();
            return cup;
        }

        public IEnumerable<Cup> GetCups()
        {
            return dbContext.Cups;
        }

        public Cup? GetCupById(int id)
        {
           return dbContext.Cups.Find(id);
        }

        public Cup? RemoveCup(int id)
        {
            Cup? cup = dbContext.Cups.Find(id);
            if (cup is not null)
            {
                dbContext.Cups.Remove(cup);
                dbContext.SaveChanges();
                return cup;
            }
            return null;
        }

        public Cup? UpdateCup(Cup updatedCup)
        {
            var existingCup = dbContext.Cups.Find(updatedCup.Id);
            if (existingCup is not null) {
                existingCup.Color = updatedCup.Color;
                existingCup.Volume = updatedCup.Volume;
                dbContext.SaveChanges();
                return existingCup;
            }
            return null;
        }
    }
}
