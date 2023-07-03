using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class CustomerOrderViewModel : ViewBase<MealMenu, Guid>
    {
        public CustomerOrderViewModel() { }
        public CustomerOrderViewModel(MealMenu model)
        {
            if (model != null)
            {
                Id = model.Id;
                DishId = model.DishId;
                MenuId = model.MenuId;



            }
        }
       
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display]
        [GridDisplay]

        public Guid? DishId { get; set; }
        [Display]
        [GridDisplay]

        public Guid? MenuId { get; set; }
        [Display]
        [GridDisplay]

        public Guid? DishCategoryId { get; set; }
        [Display]
        [GridDisplay]

        public List<SelectListItem> Menus { get; set; }
        public List<SelectListItem> Dish { get; set; }
        public List<SelectListItem> DishCategory { get; set; }
        public override MealMenu ToModel()
        {
            var customerorder = new MealMenu
            {
                Id = Id,
                DishId = DishId,
                MenuId = MenuId,


            };
            return customerorder;
        }
    }
}
