namespace RESTcup.Models
{
    public interface ICupsRepository
    {
        Cup AddCup(Cup cup);
        IEnumerable<Cup> GetCups();
        Cup? GetCupById(int id);
        Cup? RemoveCup(int id);
        Cup? UpdateCup(Cup updatedCup);
    }
}