using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HD.Station.FoodOrder.Abstractions.FeatureBuilder
{
    public interface IFoodOrderBuilder
    {
        IServiceCollection Services { get; set; }
        IConfiguration Configuration { get; set; }
    }
}
