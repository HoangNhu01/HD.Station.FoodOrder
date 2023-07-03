using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Stores
{
    public interface IOrderDeTailStore: IStore<OrderDetail, Guid>
    {
        Task<IQueryable<OrderDetail>> GetAllAsync();
        IEnumerable<OrderDetail> GetListAllAsync();
        Task<IQueryable<OrderDetail>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, OrderDetail Value)> AddEntityAsync(OrderDetail entity);
        Task<OperationResult> DeleteInAnotherRecord(Guid id);
        Task<OrderDetail> GetByCustomerIdAsync(Guid id);
        
    }
}
