using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class OrderEditViewModel : ViewBase<Order, Guid>
    {
        public OrderEditViewModel() { }
        public OrderEditViewModel(Order model)
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
        [Display(Name = "Order ID")]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }
        [Display(Name = "MenuId")]
        public Guid? MenuId { get; set; }
        [Display(Name = "Chú thích")]
        public string Note { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Display(Name = "Người đặt món")]
        public string CreatedUser { get; set; }
        [Display(Name = "Ngày đặt món")]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Người chỉnh sửa")]
        public string LastModifiedUser { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public DateTimeOffset? LastModifiedDate { get; set; }
        public List<SelectListItem> Menus { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public override Order ToModel()
        {
            var order = new Order
            {
            Id = Id,
            CustomerId = CustomerId,
            MenuId = MenuId,
            Note = Note,
            CreatedUser = CreatedUser,
            CreatedDate = CreatedDate,
            LastModifiedUser = LastModifiedUser,
            LastModifiedDate = LastModifiedDate,
            };
            return order;
        }
    }
}
