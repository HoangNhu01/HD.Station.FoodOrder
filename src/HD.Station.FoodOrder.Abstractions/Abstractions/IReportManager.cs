using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IReportManager:IManager<Report,Guid>
    {
        Task<IQueryable<Report>> GetAllAsync();
        Task<IEnumerable <Report>> GetListAllAsync();
        Task<IQueryable<Report>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, Report Value)> AddEntityAsync(Report entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<OperationResult> AddCustomerToReportAsync(Guid reportId, List<Guid> customerIds);
    }
}
