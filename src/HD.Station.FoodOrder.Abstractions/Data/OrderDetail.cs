using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class OrderDetail
    {
       

        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public bool State { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? MenuId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
