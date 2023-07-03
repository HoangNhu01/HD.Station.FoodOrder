using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class DishCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Dish> Dishes { get; set; }
    }
}
