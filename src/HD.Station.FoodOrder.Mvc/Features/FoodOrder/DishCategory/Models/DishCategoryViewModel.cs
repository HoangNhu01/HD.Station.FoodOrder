using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Http;

namespace HD.Station.FoodOrder
{
    public class DishCategoryViewModel : ViewBase<DishCategory, Guid>
    {
        public DishCategoryViewModel() {

        }
        public DishCategoryViewModel(DishCategory model)
        {
           
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                ListDish = model.Dishes.Select(x => new DishViewModel(x)).ToList();
            }
        }
        [Display]
        [GridDisplay]
       
        //[GridDisplay]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Tên loại món ăn :")]
        [GridDisplay]
        public string Name { get; set; }

        public List<DishViewModel> ListDish { get; set; }

        //public List<DishViewModel> AddDishView(DishViewModel dish)
        //{
        //    ListDish.Add(dish);
        //    return ListDish;
        //}
        
        public override DishCategory ToModel()
        {
            var dish = new DishCategory
            {
                Id = Id,
                Name = Name,
                Dishes= (ICollection<Dish>)ListDish

            };
            return dish;
        }
    }
}
