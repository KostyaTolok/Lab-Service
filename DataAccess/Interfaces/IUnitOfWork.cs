using Models.DataBaseModels;

namespace DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Order> Orders { get; }
    }
}
