using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HD.Station.FoodOrder.Abstractions.FeatureBuilder
{
    public class DefaultFoodOrderBuilder : IFoodOrderBuilder
    {
        public DefaultFoodOrderBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }
        public IServiceCollection Services { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
