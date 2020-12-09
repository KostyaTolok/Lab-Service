using Models.DataBaseModels;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShippingContext context;
        private OrdersRepository ordersRepository;
        public UnitOfWork(string storedProcedure, string connectionString)
        {
            context = new ShippingContext(storedProcedure, connectionString);
        }
        public IRepository<Order> Orders
        {
            get
            {
                if (ordersRepository==null)
                {
                    ordersRepository = new OrdersRepository(context);
                }
                return ordersRepository;
            }
        }
    }
}
