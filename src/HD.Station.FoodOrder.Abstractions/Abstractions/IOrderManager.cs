using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IOrderManager: IManager<Order,Guid>
    {
        Task<IQueryable<Order>> GetAllAsync();
        IEnumerable<Order> GetListAllAsync();
        Task<IQueryable<Order>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, Order Value)> AddEntityAsync(Order entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<Order> GetByCustomerIdAsync(Guid id);
        Task<Order> GetByMenuIdAsync(Guid id);
    }
}
