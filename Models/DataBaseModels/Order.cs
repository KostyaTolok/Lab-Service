using System;

namespace Models.DataBaseModels
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime RequiredDate { get; set; }
        public string CompanyName { get; set; }

    }
}
