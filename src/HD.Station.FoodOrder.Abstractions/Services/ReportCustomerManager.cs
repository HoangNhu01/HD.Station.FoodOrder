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
    public class ReportCustomerManager : ManagerBase<ReportCustomer,Guid>, IReportCustomerManager
    {
        private IReportCustomerStore _store;
        public ReportCustomerManager(IServiceProvider serviceProvider, IReportCustomerStore store) : base(serviceProvider, store)
        {
            _store = store;
        }
        public async Task<IQueryable<ReportCustomer>> GetAllAsync()
        {
            return await _store.GetAllAsync();
        }
        public IEnumerable<ReportCustomer> GetListAllAsync()
        {
            return _store.GetListAllAsync();
        }
        public async Task<IQueryable<ReportCustomer>> QueryIncludeFilterAsync(string filterText = null)
        {
            return await _store.QueryIncludeFilterAsync(filterText);
        }
        public async Task<(OperationResult State, ReportCustomer Value)> AddEntityAsync(ReportCustomer entity)
        {
            return await _store.AddEntityAsync(entity);
        }
        public override async Task<OperationResult> UpdateAsync(ReportCustomer entity)
        {
            return await _store.UpdateAsync(entity);
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            return await _store.DeleteInAnotherRecordAsync(id);
        }
        public async Task<ReportCustomer> GetByCustomerIdAsync(Guid id)
        {
            return await _store.GetByCustomerIdAsync(id);
        }
        public async Task<ReportCustomer> GetByReportIdAsync(Guid id)
        {
            return await _store.GetByReportIdAsync(id);
        }
    }
}
