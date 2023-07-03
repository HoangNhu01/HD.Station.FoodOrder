using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace HD.Station.FoodOrder.Abstractions.Services
{
    public class DishCategoryManager : ManagerBase<DishCategory, Guid>, IDishCategoryManager
    {
        private IDishCategoryStore _store;

        public DishCategoryManager(IServiceProvider serviceProvider, IDishCategoryStore store) : base(serviceProvider, store)
        {
            _store = store;
        }

        public async Task<IQueryable<DishCategory>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<DishCategory> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<DishCategory>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, DishCategory Value)> AddEntityAsync(DishCategory entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(DishCategory entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await (_store.DeleteInAnotherRecordAsync(id));
        }
        public async Task<DishCategory> FindByIdAsync(Guid id)
        {
            return await (_store.FindByIdAsync(id));
        }
    }
}
