using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindWebApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}