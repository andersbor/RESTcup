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

        public IEnumerable<Cup> Get(string? colorContains = null, int? minVolume = null,
            string? sortBy = null)
        {
            IEnumerable<Cup> cupsResult = new List<Cup>(cups);
            if (colorContains is not null)
            {
                cupsResult = cupsResult.Where(cup =>
                    cup.Color != null
                 && cup.Color.Contains(colorContains, StringComparison.OrdinalIgnoreCase)
                 );
            }
            if (minVolume is not null)
            {
                cupsResult = cupsResult.Where(cup => cup.Volume > minVolume);
            }

            switch (sortBy)
            {
                case null:
                    return cupsResult;
                case "color":
                case "colorAsc":
                    return cupsResult.OrderBy(cup => cup.Color);
                case "colorDesc":
                    return cupsResult.OrderByDescending(cup => cup.Color);
                case "volume":
                case "volumeAsc":
                    return cupsResult.OrderBy(cup => cup.Volume);
                case "volumeDesc":
                    return cupsResult.OrderByDescending(cup => cup.Volume);
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

        public Cup? Delete(int id)
        {
            Cup? cup = GetById(id);
            if (cup is null)
                return null;
            cups.Remove(cup);
            return cup;
        }

        public Cup? Update(int id, Cup updatedCup)
        {
            Cup? existingCup = GetById(id);
            if (existingCup is null)
                return null;
            existingCup.Color = updatedCup.Color;
            existingCup.Volume = updatedCup.Volume;
            return existingCup;
        }
    }
}