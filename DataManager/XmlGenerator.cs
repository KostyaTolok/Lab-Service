﻿using ApplicationInsights;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using Models.DTOModels;
using System.IO;
using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using DataManager.DataManagerOptions;

namespace DataManager
{
    public class XmlGenerator
    {
        private readonly AppInsights insights;
        private readonly IOrderService orderService;
        private readonly Options options;
        public XmlGenerator(Options options)
        {
            this.options = options;
            orderService = new OrderSevice(options.DataAccessOptions.StoredProcedure, options.DataAccessOptions.ConnectionString);
            insights = new AppInsights();
        }
        public void Start()
        {
            insights.InsertInsight("Служба DataManager была запущена");
            try
            {
                ConvertOrdersToXml();
            }
            catch (Exception ex)
            {
                insights.InsertInsight(ex.Message);
            }
        }
        private void ConvertOrderToXml(int id)
        {
            OrderDTO orderDTO = orderService.GetOrder(id);
            List<OrderDTO> orderDTOs = new List<OrderDTO>() { orderDTO };
            DataTable dataTable = OrdersDTOToDataTable(orderDTOs);
            dataTable.WriteXml(Path.Combine(options.PathOptions.SourcePath, options.PathOptions.XmlFileName + ".xml"));
            dataTable.WriteXmlSchema(Path.Combine(options.PathOptions.SourcePath, options.PathOptions.XsdFileName + ".xsd"));
            insights.InsertInsight("Заказ был записан в xml файл и помещен в папку source");
        }

        private void ConvertOrdersToXml()
        {
            IEnumerable<OrderDTO> orderDTOs = orderService.GetOrders();
            DataTable dataTable = OrdersDTOToDataTable(orderDTOs);
            dataTable.WriteXml(Path.Combine(options.PathOptions.SourcePath, options.PathOptions.XmlFileName + ".xml"));
            dataTable.WriteXmlSchema(Path.Combine(options.PathOptions.SourcePath, options.PathOptions.XsdFileName + ".xsd"));
            insights.InsertInsight("Заказы были записаны в xml файл и помещены в папку source");
        }

        private DataTable OrdersDTOToDataTable(IEnumerable<OrderDTO> orders)
        {
            DataTable table = new DataTable(typeof(OrderDTO).Name);

            PropertyInfo[] props = typeof(OrderDTO).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (OrderDTO order in orders)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(order, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        public void Stop()
        {
            insights.InsertInsight("Служба DataManager была остановлена");
            insights.WriteInsightsToXml();
        }
    }
}
