using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class MenuEditViewModel : ViewBase<Menu, Guid>
    {
        public MenuEditViewModel() { }

        public MenuEditViewModel(Menu model)
        {
            if (model != null)
            {
                Id = model.Id;
                Day = model.Day;
                Description = model.Description;
                CreatedUser = model.CreatedUser;
                CreatedDate = model.CreatedDate;
                LastModifiedUser = model.LastModifiedUser;
                LastModifiedDate = model.LastModifiedDate;
                
            }
        }
        [Display(Name = "Menu ID")]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Thứ")]
        public System.DayOfWeek Day { get; set; }
        [Display(Name = "Phần mô tả")]
        public  string Description { get; set; }
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Người đặt món")]
        public string CreatedUser { get; set; }
        [Display(Name = "Ngày đặt món")]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Người chỉnh sửa")]
        public string LastModifiedUser { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public DateTimeOffset? LastModifiedDate { get; set; }
        public List<Guid> Dishes { get; set; }
        public List<SelectListItem> DishList { get; set; }
        public override Menu ToModel()
        {
            var menu = new Menu
            {
            
            
                Id = Id,
                Day =Day,
                Description =Description,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate,
                LastModifiedUser = LastModifiedUser,
                LastModifiedDate = LastModifiedDate,

            
        };
            return menu;
        }
    }
}
