using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using HD.Station.FoodOrder.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace HD.Station.FoodOrderDeTail.SqlServer.Stores
{
    public class OrderDeTailStore : StoreBase<OrderDetail, Guid>, IOrderDeTailStore
    {
        private FoodOrderDbContext _dbContext;

        public OrderDeTailStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<OrderDetail>> GetAllAsync()
        {
            return _dbContext.OrderDetails.AsQueryable();
        }
        public IEnumerable<OrderDetail> GetListAllAsync()
        {
            return _dbContext.OrderDetails.Include(s => s.Dish).Include(s => s.Order).ToList();
        }

        public async Task<IQueryable<OrderDetail>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.OrderDetails.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText))).Include(s => s.Dish).Include(s => s.Order);
        }
        public async Task<(OperationResult State, OrderDetail Value)> AddEntityAsync(OrderDetail entity)
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
        public override async Task<OperationResult> UpdateAsync(OrderDetail entity)
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
        public async Task<OperationResult> DeleteInAnotherRecord(Guid id)
        {
            try
            {
                var rs = _dbContext.OrderDetails.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.OrderDetails.Remove(rs);
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }
        public async Task<OrderDetail> GetByCustomerIdAsync(Guid id)
        {
            var item = _dbContext.OrderDetails.Where(m => m.CustomerId == id).FirstOrDefault();
            return item;
        }
        public OperationResult DeleteInAnotherRecordAsync(Guid id)
        {
            try
            {
                var rs = _dbContext.OrderDetails.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.OrderDetails.Remove(rs);
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }

    }
}
