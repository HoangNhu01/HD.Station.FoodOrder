using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class Menu
    {
        public Menu()
        {
            Orders = new HashSet<Order>();
            MealMenus = new HashSet<MealMenu>();
        }
        public Guid Id { get; set; }
        public DayOfWeek Day { get; set; }    
        public string Description { get; set; }
        //public decimal UnitPrice { get; set; }
        public string CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; } 
        public string LastModifiedUser { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public virtual ICollection<MealMenu> MealMenus { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
