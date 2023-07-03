using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;
namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IMenuManager: IManager<Menu,Guid>
    {
        Task<IQueryable<Menu>> GetAllAsync();
        IEnumerable<Menu> GetListAllAsync();
        Task<IQueryable<Menu>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, Menu Value)> AddEntityAsync(Menu entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        //Task<(OperationResult State, MealMenu value)> AddDishesToMenuAsync(Guid menuId, Guid dishId);
        //Task<(OperationResult State, MealMenu value)> CreateMenuAsync(Menu menu);
        Task<OperationResult> AddDishesToMenuAsync(Guid menuId, List<Guid> dishIds);
    }
}
