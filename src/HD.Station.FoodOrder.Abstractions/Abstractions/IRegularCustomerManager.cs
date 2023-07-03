using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IRegularCustomerManager: IManager<RegularCustomer,Guid>
    {
        Task<IQueryable<RegularCustomer>> GetAllAsync();
        IEnumerable<RegularCustomer> GetListAllAsync();
        Task<IQueryable<RegularCustomer>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, RegularCustomer Value)> AddEntityAsync(RegularCustomer entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<RegularCustomer> GetByCustomerIdAsync(Guid id);
    }
}
