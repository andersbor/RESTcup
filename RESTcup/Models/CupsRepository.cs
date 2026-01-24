namespace RESTcup.Models
{
    public class CupsRepository
    {
        private int nextId = 1;
        private readonly List<Cup> cups = new();

        public CupsRepository(bool includeData = false)
        {
            if (includeData)
            {
                Add(new Cup { Color = "Red", Volume = 250 });
                Add(new Cup { Color = "Blue", Volume = 300 });
                Add(new Cup { Color = "Green", Volume = 200 });
            }
        }

        public IEnumerable<Cup> GetAll()
        {
            return new List<Cup>(cups);
        }

        public Cup Add(Cup cup)
        {
            cup.Id = nextId++;
            cups.Add(cup);
            return cup;
        }

        public Cup? GetById(int id)
        {
            return cups.FirstOrDefault(cup => cup.Id == id);
        }

        public bool Remove(int id)
        {
            Cup? cup = GetById(id);
            if (cup is null)
                return false;
            return cups.Remove(cup);
        }

        public bool Update(Cup updatedCup)
        {
            Cup? existingCup = GetById(updatedCup.Id);
            if (existingCup is null)
                return false;
            existingCup.Color = updatedCup.Color;
            existingCup.Volume = updatedCup.Volume;
            return true;
        }
    }
}
