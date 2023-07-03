using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using HD.Station.ComponentModel.DataAnnotations;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HD.Station.FoodOrder
{
    public class ReportViewModel : ViewBase<Report, Guid>
    {
        public ReportViewModel() { }
        public ReportViewModel(Report model)
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
        [Display(Name = "Id")]
        public override Guid Id { get => base.Id; set => base.Id = value; }
        [Display(Name = "Name")]
        [GridDisplay]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [GridDisplay]
        public string Description { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        [Display(Name = "Created User")]
        [GridDisplay]
        public string CreatedUser { get; set; }
        [Display(Name = "Created Date")]
        [GridDisplay]
        public DateTimeOffset CreatedDate { get; set; }
        [Display(Name = "To")]
        [GridDisplay]
        public DateTimeOffset? To { get; set; }
        [Display(Name = "From")]
        [GridDisplay]
        public DateTimeOffset? From { get; set; }
        [Display(Name = "Status")]
        [GridDisplay]
        public int Status { get; set; }
        [Display(Name = "Properties")]
        [GridDisplay]
        public string Properties { get; set; }
        [Display]
        public List<Guid> CustomerIds { get; set; }
       

        [GridDisplay]
        public string CustomerNames { get; set; }

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

