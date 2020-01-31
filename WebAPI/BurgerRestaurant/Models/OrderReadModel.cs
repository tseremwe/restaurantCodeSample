using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BurgerRestaurant.Models
{
    public class OrderReadModel
    {
        public int OrderID  {get; set;}
        public int CustomerID { get; set; }
        public IEnumerable<ItemTotal> Items { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Price { get; set; }
    }

    public class ItemTotal
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public Boolean Extras { get; set; }
        public IEnumerable<Extra> ExtraItems { get; set; }
    }
}