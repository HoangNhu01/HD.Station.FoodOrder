using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Http;

namespace HD.Station.FoodOrder
{
    public class DishCagetoryEditViewModel : ViewBase<DishCategory, Guid>
    {
        public DishCagetoryEditViewModel() { }
        public DishCagetoryEditViewModel(DishCategory model)
        {
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                

            }
        }
        [Display(Name = "DishCategory ID")]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        public override DishCategory ToModel()
        {
            var dish = new DishCategory
            {
                Id = Id,
                Name = Name
             
            };
            return dish;
        }
    }
}
