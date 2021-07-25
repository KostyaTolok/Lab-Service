using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Models.DataBaseModels;
using System;
using DataAccess.Interfaces;
using System.Linq;

namespace DataAccess
{
    public class ShippingContext : IContext
    {
        public DataSet OrdersSet { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        private string StoredProcedure { get; }
        private string ConnectionString { get; }

        public ShippingContext(string storedProcedure, string connectionString)
        {
            OrdersSet = new DataSet();
            StoredProcedure = storedProcedure;
            ConnectionString = connectionString;
        }

        public void ReadData()      //Считывание данных из базы данных
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(StoredProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(OrdersSet);
            }
        }

        public void ConvertDataSetToCollection()  //Перевести Dataset в коллекцию
        {
            Orders = OrdersSet.Tables[0].AsEnumerable().Select(dataRow => new Order
            {
                Id = dataRow.Field<int>("OrderId"),
                ProductName = dataRow.Field<string>("ProductName"),
                Quantity = dataRow.Field<short>("Quantity"),
                UnitPrice = dataRow.Field<decimal>("UnitPrice"),
                ContactName = dataRow.Field<string>("ContactName"),
                Country = dataRow.Field<string>("Country"),
                City = dataRow.Field<string>("City"),
                Address = dataRow.Field<string>("Address"),
                RequiredDate = dataRow.Field<DateTime>("RequiredDate"),
                CompanyName = dataRow.Field<string>("CompanyName")
            });
        }
    }
}
