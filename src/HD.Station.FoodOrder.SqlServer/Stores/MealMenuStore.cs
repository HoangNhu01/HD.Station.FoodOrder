using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using Microsoft.EntityFrameworkCore;

namespace HD.Station.FoodOrder.SqlServer.Stores
{
    public class MealMenuStore : StoreBase<MealMenu, Guid>, IMealMenuStore
    {
        private FoodOrderDbContext _dbContext;

        public MealMenuStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<MealMenu>> GetAllAsync()
        {
            return _dbContext.MealMenus.AsQueryable();
        }
        public IEnumerable<MealMenu> GetListAllAsync()
        {
            return _dbContext.MealMenus.Include(x => x.Menu).Include(x => x.Dish).ThenInclude(x =>x.OrderDetails).ToList();
        }

        public async Task<(OperationResult State, MealMenu Value)> AddEntityAsync(MealMenu entity)
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
        public override async Task<OperationResult> UpdateAsync(MealMenu entity)
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
                var rs = _dbContext.MealMenus.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.MealMenus.Remove(rs);
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
