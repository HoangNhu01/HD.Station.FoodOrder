using System;
using System.Collections.Generic;
using System.Linq;
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

namespace HD.Station.FoodOrder
{
    [Area("FoodOrder")]
    public class OrderController : Controller
    {
        private readonly IOrderManager _manager;
        private readonly IMenuManager _menuManager;
        private readonly IDishManager _dishManager;
        public OrderController(IOrderManager manager, IMenuManager menuManager, IDishManager dishManager)
        {
            _manager = manager;
            _menuManager = menuManager;
            _dishManager = dishManager;
        }
        public async Task<IActionResult> IndexAsync(string filter, bool includeDisabled)
        {
            var model = new OrderIndexViewModel
            {
                Filter = filter,
                IncludeDisabled = includeDisabled
            };
            //var Cus = (await _customerManager.QueryAsync()).Select(o => new SelectListItem { Text = o.FullName, Value = o.Id.ToString() }).ToList();
            var Menu = (await _menuManager.QueryAsync()).Select(o => new SelectListItem { Text = o.Description, Value = o.Id.ToString() }).ToList();
            var createModel = new OrderEditViewModel()
            {
                //Customers = Cus,
                Menus = Menu,
            };
            ViewBag.CreateModel = createModel;
            ViewBag.CountItems = (await _manager.GetAllAsync()).Count();
            return View(model);
        }
        [Permission(nameof(IndexAsync))]
        public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
        {
            try
            {
                var start = DateTime.Now;
                var items = (await _manager.QueryAsync())/*.Include(a => a.Customer)*/.Include(a => a.Menu).Select(m => new OrderViewModel
                {
                    Id = m.Id,
                    MenuId = m.MenuId,
                    DayName = m.Menu.Day.ToString(),
                    DishName = string.Join(",", m.Menu.MealMenus.Select(n => n.Dish.Name)),
                    DishNumber = m.Menu.MealMenus.Select(n => n.Dish.Name).Count(),
                    Note = m.Note,
                    CreatedDate = m.CreatedDate,
                });

                var result = await items.ToDataSourceResultAsync(request);
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
            var data = new OrderEditViewModel();
            //data.Customers = (await _customerManager.QueryAsync()).Select(o => new SelectListItem { Text = o.FullName, Value = o.Id.ToString() }).ToList();
            data.Menus = (await _menuManager.QueryAsync()).Select(o => new SelectListItem { Text = o.Description, Value = o.Id.ToString() }).ToList();
            return View(data);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync(OrderEditViewModel order)
        {
            order.CreatedDate = DateTime.Now;
            var (state, viewItem) = await _manager.AddAsync(order.ToModel());
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }
                return RedirectToAction("Details", new { id = viewItem.Id });
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                ViewBag.Notices.Add(AlertModel.Error("Order not found"));
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
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }
                return View(new OrderViewModel(viewItem));
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
            return View(new OrderViewModel());
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
                return View(new OrderViewModel(ViewItem));
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
            return View(new OrderViewModel());
        }

        [HttpGet]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeletedAsync()
        {
            var topics = new OrderViewModel();
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
        public virtual async Task<IActionResult> EditAsync(Guid id, OrderEditViewModel order)
        {
            order.LastModifiedDate = DateTime.Now;
            var (state, viewItem) = await _manager.ReadByIdAsync(id);
            if (state.Succeeded)
            {
                if (viewItem == null)
                {
                    return NotFound();
                }
                return View(new OrderEditViewModel(viewItem));
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

            return View(new OrderEditViewModel(default));
        }

        [HttpPost]
        public virtual async Task<IActionResult> EditAsync(OrderEditViewModel model)
        {
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
