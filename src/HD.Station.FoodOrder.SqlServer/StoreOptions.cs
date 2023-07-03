using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Station.FoodOrder.SqlServer
{
    public class StoreOptions
    {
        public string ConnectionName { get; set; } = "HDStation";
        public string Schema { get; set; } = "Orders";
        
    }
}
