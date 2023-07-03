using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.Abstractions.Data
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? MenuId { get; set; }
        public string Note { get; set; }
        public string CreatedUser { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public Menu Menu { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }


    }
}
