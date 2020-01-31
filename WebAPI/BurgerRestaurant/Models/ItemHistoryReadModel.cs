using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BurgerRestaurant.Models
{
    public class ItemHistoryReadModel
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Boolean Extras { get; set; }
        public DateTime Date { get; set; }
        public string MonthYear { get; set; }
    }
}