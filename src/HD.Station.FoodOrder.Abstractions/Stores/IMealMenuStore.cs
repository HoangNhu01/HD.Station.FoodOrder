using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Data;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Stores
{
    public interface IMealMenuStore : IStore<MealMenu, Guid>
    {
        Task<IQueryable<MealMenu>> GetAllAsync();
        IEnumerable<MealMenu> GetListAllAsync();
        Task<(OperationResult State, MealMenu Value)> AddEntityAsync(MealMenu entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
    }
}
