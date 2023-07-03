using System;
using System.Collections.Generic;
using System.Text;
using HD.Station.FoodOrder.Abstractions.FeatureBuilder;
using HD.Station.FoodOrder.Abstractions.Stores;
using HD.Station.FoodOrder.SqlServer.Stores;
using HD.Station.FoodOrderDeTail.SqlServer.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HD.Station.FoodOrder.SqlServer
{
    public static class SqlServerMetadataBuilderExtensions
    {
        public static IFoodOrderBuilder UseSqlServer(this IFoodOrderBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<StoreOptions>(builder.Configuration.GetSection("SqlServer")); // for inject into other services
            var options = new StoreOptions();
            builder.Configuration.GetSection("SqlServer").Bind(options);

            var connectionName = options?.ConnectionName ?? "HDStation";
            var connectionString = configuration.GetConnectionString(connectionName);

            builder.Services.AddDbContext<FoodOrderDbContext>(o => o.UseSqlServer(connectionString)); // add DBCOntext

            // add stores
            builder.Services.AddScoped<IDishStore, DishStore>();
            builder.Services.AddScoped<IDishCategoryStore, DishCategoryStore>();
            builder.Services.AddScoped<IMealMenuStore, MealMenuStore>();
            builder.Services.AddScoped<IMenuStore, MenuStore>();
            builder.Services.AddScoped<IOrderStore, OrderStore>();
            builder.Services.AddScoped<IOrderDeTailStore, OrderDeTailStore>();
            builder.Services.AddScoped<IRegularCustomerStore, RegularCustomerStore>();
            builder.Services.AddScoped<IReportCustomerStore, ReportCustomerStore>();
            builder.Services.AddScoped<IReportStore, ReportStore>();

            return builder;
        }
    }
}
