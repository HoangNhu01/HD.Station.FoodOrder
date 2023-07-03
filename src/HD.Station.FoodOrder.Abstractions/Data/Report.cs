using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class Report
    {
        public Report()
        {
            ReportCustomers = new HashSet<ReportCustomer>();
        }
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? To { get; set; }
        public DateTimeOffset? From  { get; set; }
        public int Status { get; set; }
        public string Properties { get; set; }
        public virtual ICollection<ReportCustomer> ReportCustomers { get; set; }    
    }
}
