using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.ComponentModel.DataAnnotations;
using System.Linq;

namespace HD.Station.FoodOrder
{
    public class MenuInfoViewModel : ViewBase<Menu, Guid>
    {
        public MenuInfoViewModel() { }
        public MenuInfoViewModel(Menu model)
        {
            if (model != null)
            {
                Id = model.Id;
                DayName = model.Day.StringValue();
                Description = model.Description;
                CreatedUser = model.CreatedUser;
                CreatedDate = model.CreatedDate;
                LastModifiedUser = model.LastModifiedUser;
                LastModifiedDate = model.LastModifiedDate;
                MealMenus = model.MealMenus.ToArray();
                Orders = model.Orders.ToArray();

            }
        }
        [Display]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Thứ")]
        [GridDisplay]
        public string DayName { get; set; }
        public DayOfWeek Day { get; set; }
        [Display(Name = "Mô tả")]
        [GridDisplay]
        public string Description { get; set; }
        //[Display(Name = "Đơn giá")]
        //[GridDisplay]
        //public decimal UnitPrice { get; set; }
        [Display(Name = "Ngày tạo")]
        [GridDisplay]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreatedUser { get; set; }
        [Display]
        public string LastModifiedUser { get; set; }
        [Display]
        public DateTimeOffset? LastModifiedDate { get; set; }
        [Display]
        public List<Guid> DishIds { get; set; }
        //public List<Dish> Dishes { get; set; }
        [Display]
        [GridDisplay]
        public string DishNames { get; set; }
        [Display]
        [GridDisplay]
        public int DishNumber { get; set; }
        [Display]
        public string [] DishName { get; set; }
        [Display]
        public MealMenu[] MealMenus { get; set; }
        [Display]
        public byte[] Photo { get; set; }
        [Display]
        public int OrderCount { get; set; }
        [Display]
        public Order[] Orders { get; set; }
        //public decimal TotalMoney { get; set; }
        public override Menu ToModel()
        {
            var menu = new Menu
            {
                Id = Id,
                Day = Day,
                Description = Description,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate,
                LastModifiedUser = LastModifiedUser,
                LastModifiedDate = LastModifiedDate,
            };
            return menu;
        }
    }
}
