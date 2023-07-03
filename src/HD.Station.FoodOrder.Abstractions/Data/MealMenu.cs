using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class MealMenu
    {
        public Guid Id { get; set; }
        public Guid? MenuId { get; set; }
        public Guid? DishId { get; set; }
        
        public Dish Dish { get; set; }
        public Menu Menu { get; set; }
      
    }
}
