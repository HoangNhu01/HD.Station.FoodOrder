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
    public class OrderDeTailManager : ManagerBase<OrderDetail, Guid>, IOrderDeTailManager
    {
        private IOrderDeTailStore _store;
        public OrderDeTailManager(IServiceProvider serviceProvider, IOrderDeTailStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<OrderDetail>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<OrderDetail> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<OrderDetail>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, OrderDetail Value)> AddEntityAsync(OrderDetail entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(OrderDetail entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecord(id);
        }
        public async Task<OrderDetail> GetByCustomerIdAsync(Guid id)
        {
            return await _store.GetByCustomerIdAsync(id);
        }
    }
}
