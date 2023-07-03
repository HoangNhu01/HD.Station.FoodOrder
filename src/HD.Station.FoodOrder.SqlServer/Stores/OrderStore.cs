using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using HD.Station.Security.Data;
using HD.Station.Security.Services;
using Microsoft.EntityFrameworkCore;

namespace HD.Station.FoodOrder.SqlServer.Stores
{
    public class OrderStore : StoreBase<Order, Guid>, IOrderStore
    {
        private FoodOrderDbContext _dbContext;
        private UserManager _user;

        public OrderStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<Order>> GetAllAsync()
        {
            return _dbContext.Orders.AsQueryable();
        }
        public IEnumerable<Order> GetListAllAsync()
        {
            return _dbContext.Orders.Include(s => s.OrderDetails).Include(s => s.Menu).ToList();
        }
        public async Task<IQueryable<Users>> GetListUserAllAsync()
        {
            return await _user.QueryAsync();
        }

        public async Task<IQueryable<Order>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.Orders.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText)));
        }
        public async Task<(OperationResult State, Order Value)> AddEntityAsync(Order entity)
        {
            try
            {
                var rs = await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return (OperationResult.Success, rs.Entity);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failed(ex), null);
            }
        }
        public override async Task<OperationResult> UpdateAsync(Order entity)
        {
            try
            {
                var rs = _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();
                return (OperationResult.Success);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failed(ex));
            }
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            try
            {
                var rs = _dbContext.Orders.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.Orders.Remove(rs);
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }
        public async Task<Order> GetByCustomerIdAsync(Guid id)
        {
            var item = _dbContext.Orders.Where(m => m.CustomerId == id).FirstOrDefault();
            return item;
        }
        public async Task<Order> GetByMenuIdAsync(Guid id)
        {
            var item = _dbContext.Orders.Where(m => m.MenuId == id).FirstOrDefault();
            return item;
        }
    }
}
