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
    public class MenuStore : StoreBase<Menu, Guid>, IMenuStore
    {
        private FoodOrderDbContext _dbContext;

        public MenuStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<Menu>> GetAllAsync()
        {
            return _dbContext.Menus.AsQueryable();
        }
        public IEnumerable<Menu> GetListAllAsync()
        {
            return _dbContext.Menus.Include(x => x.Orders).Include(x => x.MealMenus).ThenInclude(x => x.Dish).ToList();
        }

        //public async Task<(OperationResult State, Menu value)> CreateMenuAsync(Menu menu)
        //{
        //    try
        //    {
        //        _dbContext.Add(menu);

        //    }catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<OperationResult> AddDishesToMenuAsync(Guid menuId, List<Guid> dishIds)
        {
            var menu = _dbContext.Menus.Where(a=>a.Id == menuId).FirstOrDefault();
            var meals = new List<MealMenu>();
            foreach(var id in dishIds ?? new List<Guid>())
            {
                var mealMenu = _dbContext.MealMenus.Where(a => a.MenuId == menuId && a.DishId == id).FirstOrDefault();

                if (mealMenu == null)
                {
                    mealMenu = new MealMenu
                    {
                        DishId = id,
                        MenuId = menuId
                    };
                    meals.Add(mealMenu);
                }
            }
            if(menu != null)
            {
                try
                {
                    //add to MealMenu
                    _dbContext.MealMenus.AddRange(meals);
                    _dbContext.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return (OperationResult.Success);
        }

        public async Task<IQueryable<Menu>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.Menus.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText))).Include(a => a.Orders).Include(a => a.MealMenus).ThenInclude(a => a.Dish);
        }
        public async Task<(OperationResult State, Menu Value)> AddEntityAsync(Menu entity)
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
        public override async Task<OperationResult> UpdateAsync(Menu entity)
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
                //var rs = _dbContext.Customer.Where(m => m.Id == id);
                var rs = _dbContext.Orders.Where(m => m.MenuId == id);
                foreach (var i in rs)
                {
                    _dbContext.Orders.Remove(i);
                }
                var rc = _dbContext.MealMenus.Where(m => m.MenuId == id);
                foreach (var i in rc)
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
