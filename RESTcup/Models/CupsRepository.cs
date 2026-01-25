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

        public IEnumerable<Cup> Get(string? colorContains = null, int? volumeAtLeast = null,
            string? sortBy = null)
        {
            IEnumerable<Cup> cups2 = new List<Cup>(cups);
            if (colorContains is not null)
            {
                cups2 = cups2.Where(cup =>
                    cup.Color != null
                 && cup.Color.Contains(colorContains, StringComparison.OrdinalIgnoreCase)
                 );
            }
            if (volumeAtLeast is not null)
            {
                cups2 = cups2.Where(cup => cup.Volume > volumeAtLeast);
            }

            switch (sortBy)
            {
                case null:
                    return cups2;
                case "color":
                case "colorAsc":
                    return cups2.OrderBy(cup => cup.Color);
                case "colorDesc":
                    return cups2.OrderByDescending(cup => cup.Color);
                case "volume":
                case "volumeAsc":
                    return cups2.OrderBy(cup => cup.Volume);
                case "volumeDesc":
                    return cups2.OrderByDescending(cup => cup.Volume);
                default:
                    throw new ArgumentException("Illegal sorting: " + sortBy);
            }

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
