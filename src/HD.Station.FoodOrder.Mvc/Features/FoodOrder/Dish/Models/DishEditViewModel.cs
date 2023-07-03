using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HD.Station.FoodOrder
{
    public class DishEditViewModel : ViewBase<Dish, Guid>
    {
        public DishEditViewModel() { }

        public DishEditViewModel(Dish model)
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
                Price = model.Price;
                PhotoBase64 = Convert.ToBase64String(this.Photo);
                DishCategoryId = model.DishCategoryId;

            }
        }
        [Display(Name = "Dish ID")]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Phần mô tả")]
        public string Description { get; set; }
        [Display(Name = "Hình ảnh")]
        public byte[] Photo { get; set; }
       
        public IFormFile PhotoUpload { get; set; }
        //public string PhotoBase64 => ImageHelper.GetBase64Image(Photo);
        public string PhotoBase64 { get; set; }
        [Display(Name = "CreatedUser")]
        public string CreatedUser { get; set; }
        [Display(Name = "Ngày đặt món")]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Người chỉnh sửa")]
        public string LastModifiedUser { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public DateTimeOffset? LastModifiedDate { get; set; }
        [Display(Name = "Properties")]
        public string Properties { get; set; }
        [Display(Name = "Category")]
        public Guid DishCategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public bool Disable { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }


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
                Properties = Properties,
                Disable = Disable,
                Price = Price,
                DishCategoryId = DishCategoryId,
            };
            return dish;
        }
    }
}
