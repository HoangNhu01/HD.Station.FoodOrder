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
    public class DishCategoryStore : StoreBase<DishCategory, Guid>, IDishCategoryStore
    {
        private FoodOrderDbContext _dbContext;

        public DishCategoryStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<DishCategory>> GetAllAsync()
        {
            return _dbContext.DishCategories.AsQueryable();
        }
        public IEnumerable<DishCategory> GetListAllAsync()
        {
            return _dbContext.DishCategories.ToList();
        }

        public async Task<IQueryable<DishCategory>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.DishCategories.Where(s => string.IsNullOrEmpty(filterText) || s.Name.Contains(filterText))
                                            .Include(s => s.Dishes)
                                            .ThenInclude(x =>x.MealMenus).ThenInclude(x => x.Menu);
        }
        public async Task<(OperationResult State, DishCategory Value)> AddEntityAsync(DishCategory entity)
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
        public async Task<DishCategory> FindByIdAsync(Guid id)
        {
            return await _dbContext.DishCategories.FindAsync(id);
        }
        public override async Task<OperationResult> UpdateAsync(DishCategory entity)
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
        // xem lại delete
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            try
            {
                //var rs = _dbContext.Customer.Where(m => m.Id == id);
                var rs = _dbContext.MealMenus.Where(m => m.DishId == id);
                foreach (var i in rs)
                {
                    _dbContext.MealMenus.Remove(i);
                }
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
