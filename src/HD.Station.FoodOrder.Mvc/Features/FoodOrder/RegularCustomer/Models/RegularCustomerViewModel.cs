using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class RegularCustomerViewModel : ViewBase<RegularCustomer, Guid>
    {
        public RegularCustomerViewModel() { }
        public RegularCustomerViewModel(RegularCustomer model)
        {
            if (model != null)
            {
                Id = model.Id;
                CustomerId = model.CustomerId;
                CreatedUser = model.CreatedUser;
                CreatedDate = model.CreatedDate;
                
            }

        }
        [Hidden]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Customer Id")]   
        public Guid CustomerId { get; set; }
        [Display(Name = "Người tạo")]
        [GridDisplay]
        public string CreatedUser { get; set; }
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [GridDisplay]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "Họ và tên")]
        [GridDisplay]
        public string FullName { get; set; }
        public List<SelectListItem> Customers { set; get; }
        public override RegularCustomer ToModel()
        {
            var p = new RegularCustomer
            {
                Id = Id,
                CustomerId = CustomerId,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate
            };
            return p;
        }
    }
}

