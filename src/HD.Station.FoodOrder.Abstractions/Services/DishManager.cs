using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using HD.Station.Security.Data;

namespace HD.Station.FoodOrder.Abstractions.Services
{
    public class DishManager : ManagerBase<Dish, Guid>, IDishManager
    {
        private IDishStore _store;
        public DishManager(IServiceProvider serviceProvider, IDishStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<Dish>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<Dish> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<Dish>> QueryIncludeFilterAsync(bool includeDisabled, string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(includeDisabled, filterText);
        }
        public async Task<(OperationResult State, Dish Value)> AddEntityAsync(Dish entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(Dish entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<IQueryable<Users>> GetListUserAllAsync()
        {
            return await _store.GetListUserAllAsync();
        }
        public Dish FindByIdAsync(Guid id)
        {
            return _store.FindByIdAsync(id);
        }
        //public async Task<OperationResult> UpdateRangeAync(Dish[] dishes)
        //{
        //    try
        //    {
        //        await _store.
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
