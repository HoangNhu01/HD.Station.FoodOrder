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
    public class OrderManager : ManagerBase<Order,Guid>, IOrderManager
    {
        private IOrderStore _store;
        public OrderManager(IServiceProvider serviceProvider, IOrderStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<Order>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<Order> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<Order>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, Order Value)> AddEntityAsync(Order entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(Order entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<Order> GetByCustomerIdAsync(Guid id)
        {
            return await _store.GetByCustomerIdAsync(id);
        }
        public async Task<Order> GetByMenuIdAsync(Guid id)
        {
            return await _store.GetByMenuIdAsync(id);
        }
    }
}
