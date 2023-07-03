using System;
using System.Collections.Generic;
using System.Text;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HD.Station.FoodOrder.SqlServer
{
    public class FoodOrderDbContext : DbContextBase
    {
        
        public virtual DbSet<DishCategory> DishCategories { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<RegularCustomer> RegularCustomers { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MealMenu> MealMenus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportCustomer> ReportCustomers { get; set; }
        private IOptionsSnapshot<StoreOptions> _optionsSnapshot;
        public string Schema => _optionsSnapshot.Value?.Schema;
        //public string SchemaApp => _optionsSnapshot.Value?.SchemaApp;
        public FoodOrderDbContext(IServiceProvider serviceProvider, IOptionsSnapshot<StoreOptions> snapshot, DbContextOptions<FoodOrderDbContext> options):base (serviceProvider, options)
        {
            _optionsSnapshot = snapshot;
        }
        protected override void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>(entity =>
            //{
            //    entity.ToTable("Customer", Schema);
            //});
            modelBuilder.Entity<DishCategory>(entity =>
            {
                entity.ToTable("DishCategories", Schema);
            });
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails", Schema);
            });
            modelBuilder.Entity<RegularCustomer>(entity =>
            {
                entity.ToTable("RegularCustomer", Schema);
            });
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish", Schema);
            });
           
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu", Schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<MealMenu>(entity =>
            {
                entity.ToTable("MealMenu", Schema);
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", Schema);
            });
            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report", Schema);
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<ReportCustomer>(entity =>
            {
                entity.ToTable("ReportCustomer", Schema);
            });

            modelBuilder.Entity<MealMenu>()
                .HasOne(s => s.Dish)
                .WithMany(e => e.MealMenus)
                .HasForeignKey(s => s.DishId);
            modelBuilder.Entity<MealMenu>()
                .HasOne(s => s.Menu)
                .WithMany(e => e.MealMenus)
                .HasForeignKey(s => s.MenuId);
            //modelBuilder.Entity<RegularCustomer>()
            //    .HasOne(s => s.Customer)
            //    .WithOne(e => e.RegularCustomer)
            //    .HasForeignKey<RegularCustomer>(e => e.CustomerId);
            modelBuilder.Entity<Order>()
                .HasOne(s => s.Menu)
                .WithMany(e => e.Orders)
                .HasForeignKey(s => s.MenuId);
            //modelBuilder.Entity<Order>()
            //    .HasOne(s => s.Customer)
            //    .WithMany(e => e.Orders)
            //    .HasForeignKey(s => s.CustomerId);
            //modelBuilder.Entity<ReportCustomer>()
            //    .HasOne(s => s.Customer)
            //    .WithMany(e => e.ReportCustomers)
            //    .HasForeignKey(s => s.CustomerId);
            modelBuilder.Entity<ReportCustomer>()
                .HasOne(s => s.Report)
                .WithMany(e => e.ReportCustomers)
                .HasForeignKey(s => s.ReportId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne(s => s.Order)
                .WithMany(e => e.OrderDetails)
                .HasForeignKey(s => s.OrderId);
            modelBuilder.Entity<OrderDetail>()
               .HasOne(s => s.Dish)
               .WithMany(e => e.OrderDetails)
               .HasForeignKey(s => s.DishId);
            modelBuilder.Entity<Dish>()
                .HasOne(s => s.DishCategory)
                .WithMany(e => e.Dishes)
                .HasForeignKey(s => s.DishCategoryId);

        }
    }
}
