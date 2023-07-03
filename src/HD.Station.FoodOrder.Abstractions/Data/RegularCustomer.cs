using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class RegularCustomer
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }  
        public string CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
