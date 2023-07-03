using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class ReportCustomer
    {
        public Guid Id { get; set; }
        public Guid CustomerId  { get; set; }
        public Guid ReportId { get; set; }
        public string Properties { get; set; }
        public Report Report { get; set; }
    }
}
