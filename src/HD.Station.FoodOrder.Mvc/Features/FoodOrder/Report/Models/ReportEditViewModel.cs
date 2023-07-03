using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class ReportEditViewModel : ViewBase<Report, Guid>
    {
        public ReportEditViewModel() { }
        public ReportEditViewModel(Report model)
        {
            if (model != null)
            {
                Id = model.Id;
                Name = model.Name;
                Description = model.Description;
                OrderDate = model.OrderDate;
                CreatedUser = model.CreatedUser;
                CreatedDate = model.CreatedDate;
                To = model.To;
                From = model.From;
                Status = model.Status;
                Properties = model.Properties;
            }

        }
        [Hidden]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Customer Id")]
        public Guid CustomerId { get; set; }
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }
        [Display(Name = "Created Date")]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "To")]
        public DateTimeOffset? To { get; set; }
        [Display(Name = "From")]
        public DateTimeOffset? From { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        [Display(Name = "Properties")]
        public string Properties { get; set; }
        public List<Guid> Customers { get; set; }

        public override Report ToModel()
        {
            var p = new Report
            {
                Id = Id,
                Name = Name,
                Description = Description,
                CreatedUser = CreatedUser,
                CreatedDate = CreatedDate,
                To = To,
                From = From,
                Status = Status,
                Properties = Properties
            };
            return p;
        }
    }
}

