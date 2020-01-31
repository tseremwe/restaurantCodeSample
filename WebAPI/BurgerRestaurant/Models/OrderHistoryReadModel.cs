using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BurgerRestaurant.Models
{
    public class OrderHistoryReadModel
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public Boolean Completed { get; set; }
        public Boolean Cancelled{ get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string MonthYear { get; set; }
        public IEnumerable<Item> Items { get; set; }
        
    }
    
}