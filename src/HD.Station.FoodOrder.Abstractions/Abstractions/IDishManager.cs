using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.Security.Data;

namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IDishManager : IManager<Dish,Guid>
    {
        Task<IQueryable<Dish>> GetAllAsync();
        IEnumerable<Dish> GetListAllAsync();
        Task<IQueryable<Dish>> QueryIncludeFilterAsync(bool includeDisabled,string filterText = null);
        Task<(OperationResult State, Dish Value)> AddEntityAsync(Dish entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<IQueryable<Users>> GetListUserAllAsync();
        Dish FindByIdAsync(Guid id);
        //Task<OperationResult> UpdateRangeAync(Dish[] dishes);
    }
}
