using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Stores
{
    public interface IDishCategoryStore : IStore<DishCategory, Guid>
    {
        Task<IQueryable<DishCategory>> GetAllAsync();
        IEnumerable<DishCategory> GetListAllAsync();
        Task<IQueryable<DishCategory>> QueryIncludeFilterAsync(string filterText = null);
        Task<DishCategory> FindByIdAsync(Guid id);
        Task<(OperationResult State, DishCategory Value)> AddEntityAsync(DishCategory entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
    }
}
