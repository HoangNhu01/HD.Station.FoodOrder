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
    public class DishStore : StoreBase<Dish, Guid>, IDishStore
    {
        private FoodOrderDbContext _dbContext;
        private UserManager _user;


        public DishStore(FoodOrderDbContext dbContext, UserManager userManager, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
            _user = userManager;


        }
        public Dish FindByIdAsync(Guid id)
        {
            return  _dbContext.Dishes.Include(x => x.MealMenus).ThenInclude(x =>x.Menu).Include(x =>x.DishCategory).FirstOrDefault(x => x.Id == id);
        }
        public async Task<IQueryable<Dish>> GetAllAsync()
        {
            return _dbContext.Dishes.AsQueryable();
        }
        public IEnumerable<Dish> GetListAllAsync()
        {
            return _dbContext.Dishes.Include(s => s.DishCategory).Include(x => x.MealMenus).ToList();
        }
        public async Task<IQueryable<Users>> GetListUserAllAsync()
        {
            return await _user.QueryAsync();
        }

        public async Task<IQueryable<Dish>> QueryIncludeFilterAsync(bool includeDisabled, string filterText = null)
        {
            return _dbContext.Dishes.Where(s => (string.IsNullOrEmpty(filterText) || s.Name.Contains(filterText))).Include(s => s.DishCategory).Include(x => x.MealMenus).Include(x =>x.OrderDetails); ;
        }
        public async Task<(OperationResult State, Dish Value)> AddEntityAsync(Dish entity)
        {
            try
            {
                entity.CreatedDate = DateTimeOffset.Now;
                var rs = await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return (OperationResult.Success, rs.Entity);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failed(ex), null);
            }
        }
        public override async Task<OperationResult> UpdateAsync(Dish entity)
        {
            try
            {
                entity.LastModifiedDate = DateTimeOffset.Now;
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
