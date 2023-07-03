using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class CustomerOrderEditViewModel : ViewBase<MealMenu, Guid>
    {
        public CustomerOrderEditViewModel() { }
        public CustomerOrderEditViewModel(MealMenu model)
        {
            if (model != null)
            {
                Id = model.Id;
                DishId = model.DishId;
                MenuId = model.MenuId;
            }
        }
        [Display(Name = "CustomerOrder ID")]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        public Guid? DishId { get; set; }
        [Display(Name = "DishId")]

        public Guid? MenuId { get; set; }
        [Display(Name = "MenuId")]

        public Guid? DishCategoryId { get; set; }
        [Display(Name = "DisCategoryId")]

       
        public List<SelectListItem> Menus { get; set; }
        public List<SelectListItem> Dishs { get; set; }
        public List<SelectListItem> DishCategorys { get; set; }
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
