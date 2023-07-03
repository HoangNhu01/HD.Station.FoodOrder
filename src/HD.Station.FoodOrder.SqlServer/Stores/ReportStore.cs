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
    public class ReportStore : StoreBase<Report, Guid>, IReportStore
    {
        private FoodOrderDbContext _dbContext;

        public ReportStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<Report>> GetAllAsync()
        {
            return _dbContext.Reports.AsQueryable();
        }
        public IEnumerable<Report> GetListAllAsync()
        {
            return _dbContext.Reports.Include(a => a.ReportCustomers).ToList();
        }
        public async Task<OperationResult> AddCustomerToReportAsync(Guid reportId, List<Guid> customerIds)
        {
            var report = _dbContext.Reports.Where(a => a.Id == reportId).FirstOrDefault();
            var repCus = new List<ReportCustomer>();
            foreach (var id in customerIds)
            {
                var reportCustomer = _dbContext.ReportCustomers.Where(a => a.ReportId == reportId && a.CustomerId == id).FirstOrDefault();

                if (reportCustomer == null)
                {
                    reportCustomer = new ReportCustomer
                    {
                        CustomerId = id,
                        ReportId = reportId
                    };
                    repCus.Add(reportCustomer);
                }
            }
            if (report != null)
            {
                try
                {
                    //add to ReportCustomers
                    _dbContext.ReportCustomers.AddRange(repCus);
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return (OperationResult.Success);
        }
        public async Task<IQueryable<Report>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.Reports.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText)));
        }
        public async Task<(OperationResult State, Report Value)> AddEntityAsync(Report entity)
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
        public override async Task<OperationResult> UpdateAsync(Report entity)
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
                //var rs = _dbContext.Customer.Where(m => m.Id == id);
                var rs = _dbContext.ReportCustomers.Where(m => m.ReportId== id);
                foreach (var i in rs)
                {
                    _dbContext.ReportCustomers.Remove(i);
                }
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }
    }
}
