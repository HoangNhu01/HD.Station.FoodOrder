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
    public class RegularCustomerStore : StoreBase<RegularCustomer, Guid>, IRegularCustomerStore
    {
        private FoodOrderDbContext _dbContext;

        public RegularCustomerStore(FoodOrderDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext, serviceProvider)
        {
            _dbContext = dbContext;
        }
        public async Task<IQueryable<RegularCustomer>> GetAllAsync()
        {
            return _dbContext.RegularCustomers.AsQueryable();
        }
        public IEnumerable<RegularCustomer> GetListAllAsync()
        {
            return _dbContext.RegularCustomers.ToList();
        }

        public async Task<IQueryable<RegularCustomer>> QueryIncludeFilterAsync(string filterText = null)
        {
            return _dbContext.RegularCustomers.Where(s => (string.IsNullOrEmpty(filterText) || s.Id.ToString().Contains(filterText)));
        }
        //public async Task<IQueryable<RegularCustomer>>GetCustomerAsync(Guid id )
        //{
        //    return _dbContext.RegularCustomers.Where(s => s.CustomerId == id).Include(s=>s.Customer);
        //}


        
        public async Task<(OperationResult State, RegularCustomer Value)> AddEntityAsync(RegularCustomer entity)
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
        public override async Task<OperationResult> UpdateAsync(RegularCustomer entity)
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
                var rs = _dbContext.RegularCustomers.Where(m => m.Id == id).FirstOrDefault();
                _dbContext.RegularCustomers.Remove(rs);
                _dbContext.SaveChanges();
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex);
            }
        }
        public async Task<RegularCustomer> GetByCustomerIdAsync(Guid id)
        {
            var item =  _dbContext.RegularCustomers.Where(m => m.CustomerId == id).FirstOrDefault();
            return item;
        }
    }
}
