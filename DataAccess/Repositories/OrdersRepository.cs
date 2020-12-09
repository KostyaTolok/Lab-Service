using System.Collections.Generic;
using System.Linq;
using System.Data;
using Models.DataBaseModels;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class OrdersRepository : IRepository<Order>
    {
        private readonly ShippingContext context;

        public OrdersRepository(ShippingContext context)
        {
            this.context = context;
            FillRepository();
        }

        private void FillRepository()
        {
            context.ReadData();
            context.ConvertDataSetToCollection();
        }

        public IEnumerable<Order> Get(int id)   //Получить один заказ по id
        {
            return context.Orders.Where(order => order.Id == id);
        }

        public IEnumerable<Order> GetAll()  //Получить все заказы
        {
            return context.Orders;
        }

    }
}
