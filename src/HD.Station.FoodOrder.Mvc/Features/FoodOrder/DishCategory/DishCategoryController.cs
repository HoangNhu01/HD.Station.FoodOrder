﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;

using HD.Station.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Station.FoodOrder
{
    [Area("FoodOrder")]
    public class DishCategoryController : Controller
    {
        private readonly IDishCategoryManager _manager;
        public DishCategoryController(IDishCategoryManager manager)
        {
            _manager = manager;
        }
        public async Task<IActionResult> IndexAsync(string filter, bool includeDisabled)
        {
            var model = new DishCategoryIndexViewModel
            {
                Filter = filter,
                IncludeDisabled = includeDisabled
            };
            //ViewBag.CountItems = (await _manager.GetAllAsync()).Count();
            return View(model);
        }
        [Permission(nameof(IndexAsync))]
        public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
        {

            try
            {
               // var start = DateTime.Now;
                var query = (await _manager.QueryIncludeFilterAsync(filterText));
                var result = await query.ToDataSourceResultAsync(request);
                return Json(query);
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
            var data = new DishCagetoryEditViewModel(null);
            return View(data);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync(DishCagetoryEditViewModel dish)
        {
            {
                
                
                var (state, viewItem) = await _manager.AddAsync(dish.ToModel());
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
                    ViewBag.Notices.Add(AlertModel.Error("DishCategory not found"));
                }
                else
                {
                    ViewBag.Notice = AlertModel.Error(state.ToString());
                    return View();
                }
                return RedirectToAction("Details", new { id = viewItem.Id });
            }
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
                return View(new DishCategoryViewModel(viewItem));
            }
            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
            {
                ViewBag.Notices.Add(AlertModel.Error("DishCategory not found"));
            }
            else
            {
                ViewBag.Notice = AlertModel.Error(state.ToString());
                return View();
            }
            return View(new DishCategoryViewModel(default(DishCategory)));
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
                return View(new DishCategoryViewModel(ViewItem));
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
            return View(new DishCategoryViewModel(default(DishCategory)));
        }

        [HttpGet]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeletedAsync()
        {
            var topics = new DishCategoryViewModel(null);
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
                return View(new DishCagetoryEditViewModel(viewItem));
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

            return View(new DishCagetoryEditViewModel(default(DishCategory)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> EditAsync(DishCagetoryEditViewModel model)
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
