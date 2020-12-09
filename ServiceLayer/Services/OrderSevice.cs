using System;
using System.Collections.Generic;
using System.Linq;
using Models.DTOModels;
using ServiceLayer.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Models.DataBaseModels;

namespace ServiceLayer.Services
{
    public class OrderSevice : IOrderService
    {
        private readonly IUnitOfWork DataBase;

        public OrderSevice(string storedProcedure, string connectionString)
        {
            DataBase = new UnitOfWork(storedProcedure, connectionString);
        }

        public OrderDTO GetOrder(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id заказа не установлен. Ошибка Service Layer.");
            }
            IEnumerable<Order> orders = DataBase.Orders.Get(id.Value);
            return ConvertOrderToOrderDTO(orders);  //Вернем OrderDTO, пригодную для транспортировки
        }

        //Перевод модели заказа в модель пригодную для транспортировки на уровень DataManager
        private OrderDTO ConvertOrderToOrderDTO(IEnumerable<Order> orders)  
        {
            if (orders == null)
            {
                throw new Exception("Заказ не найден. Ошибка Service Layer.");
            }
            if (orders.Contains(null))
            {
                throw new Exception("Одно из полей заказа не установлено. Ошибка Service Layer.");
            }
            decimal totalPrice = 0;
            List<string> names = new List<string>();
            foreach (Order order in orders)
            {
                totalPrice += order.UnitPrice * order.Quantity;
                names.Add(order.ProductName);
            }
            Order firstOrder = orders.First();
            return new OrderDTO()
            {
                Address = firstOrder.Address,
                City = firstOrder.City,
                CompanyName = firstOrder.CompanyName,
                ContactName = firstOrder.ContactName,
                Country = firstOrder.Country,
                Id = firstOrder.Id,
                OrderPrice = totalPrice,
                ProductNames = names,
                RequiredDate = firstOrder.RequiredDate
            };
        }

        public IEnumerable<OrderDTO> GetOrders()
        {
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            IEnumerable<Order> orders = DataBase.Orders.GetAll();
            if (orders == null)
            {
                throw new Exception("Таблица заказов пустая. Ошибка Service Layer.");
            }
            int tempId = orders.First().Id;

            for (int i = 0; i < orders.Count();)
            {
                List<Order> oneOrder = new List<Order>();
                while (orders.ElementAt(i).Id == tempId)
                {
                    oneOrder.Add(orders.ElementAt(i));
                    i++;
                    if (i == orders.Count())
                    {
                        break;
                    }
                }
                OrderDTO orderDTO = ConvertOrderToOrderDTO(oneOrder);
                orderDTOs.Add(orderDTO);
                oneOrder.Clear();
                tempId++;
            }
            return orderDTOs;
        }
    }
}
