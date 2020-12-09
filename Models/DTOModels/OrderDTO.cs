using System;
using System.Collections.Generic;

namespace Models.DTOModels
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public List<string> ProductNames { get; set; }
        public decimal OrderPrice { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime RequiredDate { get; set; }
        public string CompanyName { get; set; }
    }
}
