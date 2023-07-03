using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;
using HD.Station.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Menu = HD.Station.FoodOrder.Abstractions.Data.Menu;

namespace HD.Station.FoodOrder
{
    [Area("FoodOrder")]
    public class MenuController : Controller
    {
        private readonly IMenuManager _manager;
        private readonly IDishManager _dishManager;
        private readonly IMealMenuManager _mealMenuManager;
        private readonly IOrderManager _orderManager;
        private readonly IDishCategoryManager _dishCategoryManager;
        public MenuController(IMenuManager manager, IDishManager dishmanager, IMealMenuManager mealMenuManager, IOrderManager orderManager, IDishCategoryManager dishCategoryManager)
        {
            _manager = manager;
            _dishManager = dishmanager;
            _mealMenuManager = mealMenuManager;
            _orderManager = orderManager;
            _dishCategoryManager = dishCategoryManager;
            _dishCategoryManager = dishCategoryManager;
        }
        public async Task<IActionResult> IndexAsync(string filter, bool includeDisabled)
        {
            
            var model = new MenuIndexViewModel
            {
                Filter = filter,
                IncludeDisabled = includeDisabled
            };
            var list = (await _dishManager.QueryAsync()).Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).ToList();
            var createModel = new MenuEditViewModel()
            {
                DishList = list
            };

            ViewBag.CreateModel = createModel;
            return View(model);
        }
        public virtual async Task<IActionResult> PaymentAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
        {
            try
            {
                var items = (await _manager.QueryAsync()).Include(a => a.MealMenus).ThenInclude(a => a.Dish);
                var day = items.Where(m => m.Day == DayOfWeek.Wednesday);
                var dishName = items.Select(m => new MenuInfoViewModel
                {
                    Id = m.Id,
                    Description = m.Description,
                    DishNumber = m.MealMenus.Select(n => n.Dish.Name).Count(),
                    DishNames = string.Join(",", m.MealMenus.Select(n => n.Dish.Name)),
                    Day = m.Day,
                    OrderCount = m.Orders.Select(n => n.MenuId).Count(),
                    //TotalMoney = (m.UnitPrice)*(m.Orders.Select(n => n.MenuId).Count())
                });
                //var result = await dishName.ToDataSourceResultAsync(request);
                return View(dishName);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message, ex.StackTrace });
            }
        }
        public virtual async Task<IActionResult> OrderAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
        {
            try
            {
                var items = (await _manager.QueryAsync()).Include(a => a.MealMenus).ThenInclude(a => a.Dish);
                //var menus = items.Where(a => a.Day == DateTimeOffset.Now.DayOfWeek).ToArray();
                //foreach (var menu in menus)
                //{
                //    var dishIds = (await _mealMenuManager.QueryAsync()).Where(a => a.MenuId == menu.Id).Select(a=>a.DishId);
                //}
              
                var dishName = items.Where(m => m.Day == DateTime.Today.DayOfWeek).Select(m => new MenuInfoViewModel
                {
                    Id = m.Id,
                    Description = m.Description,
                    DishNumber = m.MealMenus.Select(n => n.Dish.Name).Count(),
                    DishName =  m.MealMenus.Select(n => n.Dish.Name).ToArray(),
                    Day=m.Day,
                    MealMenus = m.MealMenus.ToArray(),
                    //Photo = m.MealMenus.Select(n => n.Dish.Photo),
                }).ToList();
                //var result = await dishName.ToDataSourceResultAsync(request);
                return View(dishName);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message, ex.StackTrace });
            }
        }

        [Permission(nameof(IndexAsync))]
       public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
       {
           try
            {
                var start = DateTime.Now;
                var items = (await _manager.QueryAsync()).Include(a => a.MealMenus);
                var dishName = items.ThenInclude(a => a.Dish).Select(m => new MenuInfoViewModel
                {
                    Id = m.Id,
                    DayName = m.Day.ToString(),
                    Description = m.Description,
                    DishNames = string.Join(",", m.MealMenus.Select(n => n.Dish.Name)),
                    DishNumber = m.MealMenus.Select(n => n.Dish.Name).Count()
                });
                var result = await dishName.ToDataSourceResultAsync(request);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message, ex.StackTrace });
            }
       }

        [Permission("Create")]
        [HttpGet]
        public virtual async Task<IActionResult> CreateAsync()
        {
            
            var items =  _manager.GetListAllAsync();
            var dishes = _dishManager.GetListAllAsync();
            ViewBag.Dishes = dishes.Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() }).ToList();
            var data = new MenuEditViewModel();
            return View(data);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync(MenuEditViewModel menu)
        {
            menu.Id = Guid.NewGuid();
            menu.CreatedDate = DateTimeOffset.Now;
            var (state, viewItem) = await _manager.AddAsync(menu.ToModel());
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }

                var stateAddDishes = await _manager.AddDishesToMenuAsync(menu.Id, menu.Dishes);
                if (stateAddDishes.Exception != null)
                {
                    ViewBag.Notices.Add(AlertModel.Error(" AddDishesToMenu-Failed!"));
                }
                return RedirectToAction("Details", new { id = viewItem.Id });
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                ViewBag.Notices.Add(AlertModel.Error("Menu not found"));
            }
            else
            {
                ViewBag.Notice = AlertModel.Error(state.ToString());
                return View();
            }
            return RedirectToAction("Details", new { id = viewItem.Id });
        }

        [HttpGet("[area]/[controller]/{id:guid}")]
        public virtual async Task<IActionResult> DetailsAsync(Guid id)
        {
            var (state, viewItem) = await _manager.ReadByIdAsync(id);

            ViewBag.Id = id;
            var items = (await _manager.QueryAsync()).Where(m => m.Id == id).Include(a => a.MealMenus).ThenInclude(a => a.Dish).Select(m => new MenuInfoViewModel
            {
                Id = m.Id,
                DayName = m.Day.ToString(),
                Description = m.Description,
                DishNames = string.Join(",", m.MealMenus.Select(n => n.Dish.Name)),
                DishNumber = m.MealMenus.Select(n => n.Dish.Name).Count(),
                DishName = m.MealMenus.Select(n => n.Dish.Name).ToArray(),

            }).FirstOrDefault();
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }
                return View(items);
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                ViewBag.Notices.Add(AlertModel.Error("Dish not found"));
            }
            else
            {
                ViewBag.Notice = AlertModel.Error(state.ToString());
                return View();
            }
            return View(new MenuInfoViewModel(default));
        }

        [HttpGet]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeleteAsync(Guid id, string message, string urlReturn)
        {
            ViewBag.Id = id;
            var listNotice = new List<NoticeModel>();
            if (!string.IsNullOrWhiteSpace(message))
            {
                listNotice.Add(JsonConvert.DeserializeObject<AlertModel>(message));
            }
            ViewBag.Notices = listNotice;
            ViewData["ReturnUrl"] = urlReturn;
            var (State, ViewItem) = await _manager.DeletePreCheckAsync(id);
            if (State.Succeeded)
            {
                if (ViewItem == null)
                {
                    return NotFound();
                }
                if (State.Message != null)
                {
                    ViewBag.Notices.Add(AlertModel.Warning(State.ToString()));
                }
                return View(new MenuInfoViewModel(ViewItem));
            }
            else if (State.Exception != null && State.Exception is UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            else if (ViewItem == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Notices.Add(AlertModel.Error(State.ToString()));
            }
            return View(new MenuInfoViewModel(default));
        }

        [HttpGet]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeletedAsync()
        {
            var topics = new MenuInfoViewModel(null);
            ViewBag.Notice = new AlertModel()
            {
                Contextual = "success",
                Title = "Successed",
                Message = "Deleted!"
            };
            return View(topics);
        }

        [HttpPost]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeleteConfirmedAsync([FromRoute] Guid id)
        {
            var stateAnother = await _manager.DeleteInAnotherRecordAsync(id);
            var state = await _manager.DeleteByIdAsync(id);
            if (state.Succeeded)
            {
                return RedirectToAction("Deleted");
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            return RedirectToAction("Delete", new { id = id, message = JsonConvert.SerializeObject(state.ToAlert()) });
        }
        [Permission("Edit")]
        [HttpGet]
        public virtual async Task<IActionResult> EditAsync(Guid id)
        {
            
            var (state, viewItem) = await _manager.ReadByIdAsync(id);
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }
                return View(new MenuEditViewModel(viewItem));
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                ViewBag.Notices.Add(AlertModel.Error("not found"));
            }
            else
            {
                ViewBag.Notice = AlertModel.Error(state.ToString());
                return View();
            }

            return View(new MenuEditViewModel(default));
        }

        [HttpPost]
        public virtual async Task<IActionResult> EditAsync(MenuEditViewModel model)
        {
            model.LastModifiedDate = DateTime.Now;
            var vm = model.ToModel();
            var state = await _manager.UpdateAsync(vm);
            if (state.Succeeded)
            {
                ViewBag.Notice = new AlertModel()
                {
                    Contextual = "success",
                    Title = "Thành công",
                    Message = "Đã sửa thành công!"
                };

                return RedirectToAction("Details", new { Id = vm.Id });
            }
            ViewBag.Notice = state.ToAlert();
            return View(model);
        }
    }
}
