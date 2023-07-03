using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;

namespace HD.Station.FoodOrder.Abstractions.Services
{
    public class MealMenuManager : ManagerBase<MealMenu,Guid> , IMealMenuManager
    {
        private IMealMenuStore _store;
        public MealMenuManager(IServiceProvider serviceProvider, IMealMenuStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<MealMenu>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<MealMenu> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<(OperationResult State, MealMenu Value)> AddEntityAsync(MealMenu entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(MealMenu entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
    }
}
