using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class RegularCustomerEditViewModel: ViewBase<RegularCustomer, Guid>
    {
        public RegularCustomerEditViewModel() { }
        public RegularCustomerEditViewModel(RegularCustomer model)
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
        [Display(Name="Customer Id")]
        public Guid CustomerId { get; set; }
        [Display(Name="Created User")]
        public string CreatedUser { get; set; }
        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set;}
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

