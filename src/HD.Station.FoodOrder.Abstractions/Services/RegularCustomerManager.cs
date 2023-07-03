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
    public class RegularCustomerManager: ManagerBase<RegularCustomer, Guid>, IRegularCustomerManager
    {
        private IRegularCustomerStore _store;
        public RegularCustomerManager(IServiceProvider serviceProvider, IRegularCustomerStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<RegularCustomer>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<RegularCustomer> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<RegularCustomer>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, RegularCustomer Value)> AddEntityAsync(RegularCustomer entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(RegularCustomer entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<RegularCustomer> GetByCustomerIdAsync(Guid id)
        {
            return await _store.GetByCustomerIdAsync(id);
        }
    }
}
