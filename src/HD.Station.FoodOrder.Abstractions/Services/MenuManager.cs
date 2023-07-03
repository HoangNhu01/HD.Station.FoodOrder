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
    public class MenuManager: ManagerBase<Menu, Guid>, IMenuManager
    {
        private IMenuStore _store;
        public MenuManager(IServiceProvider serviceProvider, IMenuStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<Menu>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<Menu> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<Menu>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, Menu Value)> AddEntityAsync(Menu entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(Menu entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<OperationResult> AddDishesToMenuAsync(Guid menuId, List<Guid> dishIds)
            => await _store.AddDishesToMenuAsync(menuId, dishIds);
 


    }
}
