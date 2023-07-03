using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Http;

namespace HD.Station.FoodOrder
{
    public class DishViewModel : ViewBase<Dish, Guid>
    {
        public DishViewModel() { }
        public DishViewModel(Dish model)
        {
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                Description = model.Description;
                Photo = model.Photo;
                CreatedDate = model.CreatedDate;
                CreatedUser = model.CreatedUser;
                LastModifiedDate = model.LastModifiedDate;
                LastModifiedUser = model.LastModifiedUser;
                CategoryName = model.DishCategory?.Name;
                Disable = model.Disable;
                Price= model.Price;
                PhotoBase64 = Convert.ToBase64String(this.Photo);
                DishCategoryId = model.DishCategoryId;
                MealMenu = model.MealMenus.FirstOrDefault(x => x.Menu.Day.IntValue() == DateTime.Today.DayOfWeek.IntValue()); ;
            }
        }
        [Display]
        [GridDisplay]
        public byte[] Photo { get; set; }
        //[GridDisplay]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Tên món ăn")]
        [GridDisplay]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        [GridDisplay]
        public string Description { get; set; }
        [Display]
        public string CreatedUser { get; set; }
        [Display(Name = "Ngày tạo")]
        [GridDisplay]
        public DateTimeOffset CreatedDate { get; set; }
        [Display]
        public string LastModifiedUser { get; set; }
        [Display]
        public DateTimeOffset? LastModifiedDate { get; set; }
        [Display]
        public string Properties { get; set; }
        public IFormFile PhotoUpload { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        //public string PhotoBase64 => ImageHelper.GetBase64Image(Photo);
        public string PhotoBase64 { get; set; }
        public bool Disable { get; set; }
        public Guid DishCategoryId { get; set; }
        public MealMenu MealMenu{ get; set; }
        public override Dish ToModel()
        {
            var dish = new Dish
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Photo = Photo,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate,
                LastModifiedUser = LastModifiedUser,
                LastModifiedDate = LastModifiedDate,
                Properties= Properties,
                Disable= Disable,
                Price = Price,
                DishCategoryId= DishCategoryId,
            };
            return dish;
        }

    }
}


