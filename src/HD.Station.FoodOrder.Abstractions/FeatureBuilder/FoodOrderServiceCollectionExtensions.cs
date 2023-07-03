using System;
using System.Collections.Generic;
using System.Text;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HD.Station.FoodOrder.Abstractions.FeatureBuilder
{
    public static class FoodOrderServiceCollectionExtensions
    {
        public static IFoodOrderBuilder AddFoodOrderFeature(this IServiceCollection services, IConfiguration configuration)
        {
            return new DefaultFoodOrderBuilder(services, configuration);
        }
        public static IFoodOrderBuilder AddManagers(this IFoodOrderBuilder builder)
        {
            builder.Services.AddScoped<IDishCategoryManager, DishCategoryManager>();
            builder.Services.AddScoped<IDishManager, DishManager>();
            builder.Services.AddScoped<IDishCategoryManager, DishCategoryManager>();
            builder.Services.AddScoped<IMealMenuManager, MealMenuManager>();
            builder.Services.AddScoped<IMenuManager, MenuManager>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();
            builder.Services.AddScoped<IOrderDeTailManager, OrderDeTailManager>();
            builder.Services.AddScoped<IRegularCustomerManager, RegularCustomerManager>();
            builder.Services.AddScoped<IReportCustomerManager, ReportCustomerManager>();
            builder.Services.AddScoped<IReportManager, ReportManager>();

            return builder;
        }
    }
}
