namespace RESTcup.Models
{
    public class Cup
    {
        public int Id { get; set; }
        public string? Color { get; set; }
        public int Volume { get; set; }

        public override string ToString()
        {
            return $"Cup(Id={Id}, Color={Color}, Volume={Volume})";
        }

        override public bool Equals(object? obj)
        {
            if (obj is not Cup other)
                return false;
            return Id == other.Id;
        }

        override public int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
