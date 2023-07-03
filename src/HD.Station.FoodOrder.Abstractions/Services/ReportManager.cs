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
    public class ReportManager : ManagerBase<Report, Guid>, IReportManager
    {
        private IReportStore _store;
        public ReportManager(IServiceProvider serviceProvider, IReportStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<Report>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public async Task<IEnumerable<Report>> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<Report>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, Report Value)> AddEntityAsync(Report entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(Report entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<OperationResult> AddCustomerToReportAsync(Guid reportId, List<Guid> customerIds)     
              => await _store.AddCustomerToReportAsync(reportId, customerIds);
        
    }
}
