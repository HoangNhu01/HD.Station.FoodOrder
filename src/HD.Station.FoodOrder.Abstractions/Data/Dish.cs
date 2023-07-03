using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class Dish
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public  byte[] Photo { get; set; }
        public string CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string Properties { get; set; }
        public Guid DishCategoryId { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<MealMenu> MealMenus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public DishCategory DishCategory { get; set; }
        public bool Disable { get; set; }
    }
}
