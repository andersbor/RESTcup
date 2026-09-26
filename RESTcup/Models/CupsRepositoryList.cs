namespace RESTcup.Models
{
    public class CupsRepositoryList : ICupsRepository
    {
        private int nextId = 1;
        private readonly List<Cup> cups = new();

        public CupsRepositoryList(bool includeData = false)
        {
            if (includeData)
            {
                AddCup(new Cup { Color = "Red", Volume = 250 });
                AddCup(new Cup { Color = "Blue", Volume = 300 });
                AddCup(new Cup { Color = "Green", Volume = 200 });
            }
        }

        public IEnumerable<Cup> GetCups()
        {
            return new List<Cup>(cups);
        }

        public Cup AddCup(Cup cup)
        {
            cup.Id = nextId++;
            cups.Add(cup);
            return cup;
        }

        public Cup? GetCupById(int id)
        {
            return cups.FirstOrDefault(cup => cup.Id == id);
        }

        public Cup? RemoveCup(int id)
        {
            Cup? cup = GetCupById(id);
            if (cup is null)
                return null;
            cups.Remove(cup);
            return cup;
        }

        public Cup? UpdateCup(Cup updatedCup)
        {
            Cup? existingCup = GetCupById(updatedCup.Id);
            if (existingCup is null)
                return null;
            existingCup.Color = updatedCup.Color;
            existingCup.Volume = updatedCup.Volume;
            return existingCup;
        }
    }
}
