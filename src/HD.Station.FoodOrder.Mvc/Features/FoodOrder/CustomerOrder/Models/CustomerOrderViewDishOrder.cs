using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Linq;

namespace HD.Station.FoodOrder
{
    public class CustomerOrderViewDishOrder : ViewBase<OrderDetail, Guid>
    {
        public CustomerOrderViewDishOrder()
        {
            
        }
        public CustomerOrderViewDishOrder(OrderDetail model)
        {
            if (model != null)
            {
                Id = model.Id;
                DishId = model.DishId;
                OrderId = model.OrderId;
                Quantity = model.Quantity;
                State = model.State;
                Dish = model.Dish;
                Order = model.Order;
                MenuId = model.MenuId;

            }
        }
       
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display]
        [GridDisplay]

        public Guid DishId { get; set; }
        [Display]
        [GridDisplay]

        public Guid? OrderId { get; set; }
        [Display]
        [GridDisplay]

        public int Quantity { get; set; }
        [Display]
        [GridDisplay]

        public bool State { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Order Order { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Dish Dish { get; set; }
        public virtual Guid? MenuId { get; set; }
        public override OrderDetail ToModel()
        {
            var customerorder = new OrderDetail
            {
                Id = Id,
                DishId = DishId,
                OrderId = OrderId,
                Quantity = Quantity,
                State = State,
                Dish = Dish,
                Order = Order,
                MenuId = MenuId,

            };
            return customerorder;
        }
    }
}
