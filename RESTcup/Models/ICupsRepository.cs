namespace RESTcup.Models
{
    public interface ICupsRepository
    {
        Cup Add(Cup cup);
        IEnumerable<Cup> GetAll();
        Cup? GetById(int id);
        bool Remove(int id);
        bool Update(Cup updatedCup);
    }
}