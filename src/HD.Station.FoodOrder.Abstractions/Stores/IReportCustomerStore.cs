using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Stores
{
    public interface IReportCustomerStore: IStore<ReportCustomer, Guid>
    {
        Task<IQueryable<ReportCustomer>> GetAllAsync();
        IEnumerable<ReportCustomer> GetListAllAsync();
        Task<IQueryable<ReportCustomer>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, ReportCustomer Value)> AddEntityAsync(ReportCustomer entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<ReportCustomer> GetByCustomerIdAsync(Guid id);
        Task<ReportCustomer> GetByReportIdAsync(Guid id);
    }
}
