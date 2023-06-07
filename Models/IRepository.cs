namespace WebApiDB.Models
{
    public interface IRepository
    {
        IEnumerable<Dealer> GetAll();
        Dealer Get(int id);
        void Create(Dealer dealer);
    }
}
