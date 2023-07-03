using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class OrderViewModel : ViewBase<Order, Guid>
    {
        public OrderViewModel() { }
        public OrderViewModel(string detail)
        {
            Id = Guid.NewGuid();
            if(detail =="")
            {
                MenuId = null;
            }
            else { MenuId = Guid.Parse(detail); }          
            CreatedDate = DateTimeOffset.Now;
        }

        public OrderViewModel(Order model)
        {
            if (model != null)
            {
                Id = model.Id;
                CustomerId = model.CustomerId;
                MenuId = model.MenuId;
                Note = model.Note;
                CreatedUser = model.CreatedUser;
                CreatedDate = model.CreatedDate;
                LastModifiedUser = model.LastModifiedUser;
                LastModifiedDate = model.LastModifiedDate;
            }
        }
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display]
        [GridDisplay]
        public Guid? CustomerId { get; set; }
        [Display]
        public Guid? MenuId { get; set; }
        [Display]
        [GridDisplay]
        public string DayName { get; set; }
        [Display]
        [GridDisplay]
        public string FullName { get; set; }
        [Display]
        [GridDisplay]
        public string DishName { get; set; }
        [Display]
        [GridDisplay]
        public int DishNumber { get; set; }
        [Display]
        [GridDisplay]
        public string Note { get; set; }
        [Display]
        [GridDisplay]
        public int Quantity { get; set; }
        [Display]
        [GridDisplay]
        public decimal UnitPrice { get; set; }
        [Display]
       
        public int ToTalQuantity { get; set; }
        [Display]

        public int TongOrder { get; set; }

        [Display]

        public decimal TotalPrice { get; set; }
        [Display]
        public string CreatedUser { get; set; }
        [Display]
        [GridDisplay]
        public DateTimeOffset CreatedDate { get; set; }
        [Display]
       
        public string LastModifiedUser { get; set; }
        [Display]
       
        public DateTimeOffset? LastModifiedDate { get; set; }
        public List<SelectList> Menus { get; set; }
        public List<SelectList> Customers { get; set; }
        
        public override Order ToModel()
        {
            var order = new Order
            {
                Id = Id,
                CustomerId = CustomerId,
                MenuId = MenuId,
                Note = Note,
                TotalPrice = TotalPrice,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate,
                LastModifiedUser = LastModifiedUser,
                LastModifiedDate = LastModifiedDate,
            };
            return order;
        }
    }
}
