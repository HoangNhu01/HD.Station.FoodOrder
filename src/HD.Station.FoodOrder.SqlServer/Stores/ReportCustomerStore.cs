using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Stores;
using Microsoft.EntityFrameworkCore;

namespace HD.Station.FoodOrder.SqlServer.Stores
{
    public class ReportCustomerStore : StoreBase<ReportCustomer, Guid>, IReportCustomerStore
    {
        private FoodOrderDbContext _dbContext;

        public ReportCustomerStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<ReportCustomer>> GetAllAsync()
        {
            return _dbContext.ReportCustomers.AsQueryable();
        }
        public IEnumerable<ReportCustomer> GetListAllAsync()
        {
            return _dbContext.ReportCustomers.ToList();
        }

        public async Task<IQueryable<ReportCustomer>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.ReportCustomers.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText)));
        }
        public async Task<(OperationResult State, ReportCustomer Value)> AddEntityAsync(ReportCustomer entity)
        {
            try
            {
                var rs = await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return (OperationResult.Success, rs.Entity);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failed(ex), null);
            }
        }
        public override async Task<OperationResult> UpdateAsync(ReportCustomer entity)
        {
            try
            {
                var rs = _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();
                return (OperationResult.Success);
            }
            catch (Exception ex)
            {
                return (OperationResult.Failed(ex));
            }
        }
        public async Task<OperationResult> DeleteInAnotherRecordAsync(Guid id)
        {
            try
            {
                var rs = _dbContext.ReportCustomers.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.ReportCustomers.Remove(rs);
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }
        public async Task<ReportCustomer> GetByCustomerIdAsync(Guid id)
        {
            var item = _dbContext.ReportCustomers.Where(m => m.CustomerId == id).FirstOrDefault();
            return item;
        }
        public async Task<ReportCustomer> GetByReportIdAsync(Guid id)
        {
            var item = _dbContext.ReportCustomers.Where(m => m.ReportId == id).FirstOrDefault();
            return item;
        }
    }
}
