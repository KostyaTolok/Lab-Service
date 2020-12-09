using System;
using System.Collections.Generic;
using Models.DTOModels;

namespace ServiceLayer.Interfaces
{
    public interface IOrderService
    {
        OrderDTO GetOrder(int? id);
        IEnumerable<OrderDTO> GetOrders();
    }
}
