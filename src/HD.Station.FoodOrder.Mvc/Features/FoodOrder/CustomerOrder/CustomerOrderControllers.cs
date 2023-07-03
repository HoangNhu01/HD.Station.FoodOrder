using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.FoodOrder.Abstractions.Services;
using HD.Station.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using HD.Station.HumanResource;
using HD.Station.Security.Data;
using Microsoft.AspNetCore.Http;




namespace HD.Station.FoodOrder
{
    [Area("FoodOrder")]
   
    public class CustomerOrderController : Controller {
        private readonly IDishManager _manager;
        private readonly IDishCategoryManager _categoryManager;
        private readonly IOrderDeTailManager _orderDeTailManager;
        private readonly IMenuManager _menuManager;
        private readonly IOrderManager _orderManager;
        private readonly IMealMenuManager _mealMenuManager;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CustomerOrderController(IDishManager manager,
                                       IDishCategoryManager categoryManager,
                                       IMenuManager menuManager,
                                       IOrderDeTailManager orderDeTailManager,
                                       IOrderManager orderManager,
                                       IMealMenuManager mealMenuManager,
                                       ICurrentUserAccessor currentUserAccessor)
        {
            _manager = manager;
            _categoryManager = categoryManager;
            _menuManager = menuManager;
            _orderDeTailManager = orderDeTailManager;
            _orderManager = orderManager;
            _mealMenuManager = mealMenuManager;
            _currentUserAccessor = currentUserAccessor;
        }
        
        public async Task<IActionResult> IndexAsync(string filterText = null, bool includeDisabled = false, string message = null)
        {
            //var user = HttpContext.User.Identity.Name;
            //var userID = _currentUserAccessor.UserId.ToString();
            //var Id = _currentUserAccessor.ClaimsPrincipal.GetIdentifier();
            var categories = (await _categoryManager.QueryIncludeFilterAsync(filterText))
                                                    .Select(x => new DishCategoryViewModel(x));
            if(message != null)
            {
                ViewBag.Message = message;
            }
            return View(categories);
        }
        public async Task<IActionResult> MenuIndexAsync(string filterText = null)
        {
            var listNameMenu = (await _menuManager.QueryIncludeFilterAsync(filterText))
                                          .Where(x => x.Day.IntValue() == DateTime.Today.DayOfWeek.IntValue())
                                          .Select(x => new MenuInfoViewModel(x));
            
            return View(listNameMenu);
        }
      
        public async Task<IActionResult> OrderDeTailAsync(Guid id)
        {
            var view = new DishViewModel((_manager.FindByIdAsync(id)));
           
            return View(view);
        }
        [HttpGet]
        [Permission(nameof(IndexAsync))]
        public virtual async Task<IActionResult> ReadAsync(/*[DataSourceRequest] DataSourceRequest request*/ string filterText = null, bool includeDisabled = false)
        {

            try
            {
                //var start = DateTime.Now;
                //var dishes = (await _manager.QueryIncludeFilterAsync(includeDisabled, filterText)).Select(m => new DishViewModel(m));
                var categories = (await _categoryManager.QueryIncludeFilterAsync(filterText)).Select(x => new DishCategoryViewModel(x));
                return Json(new { ListDish = categories });
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message, ex.StackTrace });
            }
        }
        public async Task<IActionResult> MyActionAsync(CustomerOrderViewDishOrder data)
        {
            //var model =  JsonConvert.DeserializeObject<CustomerOrderViewDishOrder>(HttpUtility.UrlDecode(data);
            //model.OrderId = Guid.NewGuid();
           
            var (state, viewItem) = await _orderDeTailManager.AddAsync(data.ToModel());
            // do something with the model
            return Json("Thêm vào giỏ hàng thành công! Vui lòng check giỏ hàng.");
        }
        public async Task<IActionResult> AddOrderForMenuAsync(string id = null)
        {
            if (id.Length > 0)
            {               
                var Id = Guid.Parse(id);
                var menuOrder = _mealMenuManager.GetListAllAsync().Where(x => x.MenuId == Id && x.Dish.Disable == false).Select(x => new CustomerOrderViewDishOrder()
                {
                    Id = Guid.NewGuid(),
                    DishId = (Guid)x.Dish.Id,
                    Quantity = 1,
                    Dish = x.Dish,
                    MenuId = (Guid)x.MenuId,
                });
                foreach (var order in menuOrder)
                {
                    var (state, item) = await _orderDeTailManager.AddEntityAsync(order.ToModel());

                }
            }
            return RedirectToAction("MyCart", new { id = id });
        }
  
        public async Task<IActionResult> MyCartAsync(string id = null)
        {
            var model = _orderDeTailManager.GetListAllAsync();
            var dishOrder = model.Where(x => x.State == false && x.Dish.Disable == false).Select(x => new CustomerOrderViewDishOrder(x));
            if (dishOrder.Count() ==0)
            {
                return RedirectToAction("Index", new { message = "Giỏ hàng đang trống hoặc đã được thanh toán hết, vui lòng đặt món!" });
            }
            //if (id != null)
            //{
            //    var Id = Guid.Parse(id);
            //    var menuOrder = dishOrder.Where(x => x.MenuId == Id);          
            //    ViewBag.IsMenu = true;
            //    return View(menuOrder);

            //}
            return View(dishOrder);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(/*Guid[]*/IEnumerable<CustomerOrderViewDishOrder> data)
        {
            var menuIds = data.Select(x => x.MenuId.ToString()).ToList();
            var menuid = menuIds.Distinct().ToList();
            
            foreach (var id in menuid)
            {

                var newOrder = new OrderViewModel(id);
                //var changeStateOrder = _orderDeTailManager.GetListAllAsync().FirstOrDefault(a => a.Id == id && a.State == false && a.MenuId.ToString() == menuId);
                //changeStateOrder.State = !changeStateOrder.State;
                var newData = data.Where(x => x.MenuId.ToString() == id).Select(x => new CustomerOrderViewDishOrder()
                {
                    Id = x.Id,
                    MenuId = x.MenuId,
                    DishId = x.DishId,
                    State = !x.State,
                    OrderId = newOrder.Id
                });
                
                

                var (state, viewItem) = await _orderManager.AddEntityAsync(newOrder.ToModel());
                foreach(var item in newData)
                {
                    var r = await _orderDeTailManager.UpdateAsync(item.ToModel());
                }
            }
            
           
            return Json("Order thanh cong");
        }

        public async Task<IActionResult> DeleteOrderDeTailAsync(string id, string menuId = null )
        {
            var delete = await _orderDeTailManager.DeleteInAnotherRecordAsync(Guid.Parse(id));
            return RedirectToAction("MyCart", new {id = menuId});
        }
    }

}
