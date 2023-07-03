using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Stores
{
    public interface IReportStore : IStore<Report,Guid>
    {
        Task<IQueryable<Report>> GetAllAsync();
        IEnumerable<Report> GetListAllAsync();
        Task<IQueryable<Report>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, Report Value)> AddEntityAsync(Report entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<OperationResult> AddCustomerToReportAsync(Guid reportId, List<Guid> customerIds);
    }
}
