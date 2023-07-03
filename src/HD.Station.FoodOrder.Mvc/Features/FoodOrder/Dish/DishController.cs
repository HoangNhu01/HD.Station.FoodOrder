using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Abstractions;
using HD.Station.FoodOrder.Abstractions.Data;

using HD.Station.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HD.Station.FoodOrder
{
    [Area("FoodOrder")]
    public class DishController : Controller
    {
        private readonly IDishManager _manager;
        private readonly IDishCategoryManager _dishCategoryManager;
        public DishController(IDishManager manager, IDishCategoryManager dishCategoryManager)
        {
            _manager = manager;
            _dishCategoryManager = dishCategoryManager;
        }
        public async Task<IActionResult> IndexAsync(string filter, bool includeDisabled)
        {
            var model = new DishIndexViewModel
            {
                Filter = filter,
                IncludeDisabled = includeDisabled
            };
            var listuser = await _manager.GetListUserAllAsync();
            ViewData["UserName"] = new SelectList(listuser.ToList(), "Id", "UserName");
            ViewData["DishCategoryName"] = new SelectList(_dishCategoryManager.GetListAllAsync().ToList(), "Id", "Name");
            ViewBag.CountItems = (await _manager.GetAllAsync()).Count();
            return View(model);
        }
        [HttpPost]
        [Permission(nameof(IndexAsync))]
        public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText, [FromForm] bool includeDisabled)
        {

            try
            {
                var start = DateTime.Now;
                var query = (await _manager.QueryIncludeFilterAsync(includeDisabled, filterText)).Select(m => new DishViewModel(m));

                var result = await query.ToDataSourceResultAsync(request);
                return Json(query);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message, ex.StackTrace });
            }
        }
        [HttpPost]

        public async Task<IActionResult> ChangeStateAsync(Guid[] selectedIds)
        {
            try
            {

                foreach (var id in selectedIds)
                {
                    var enableDish = _manager.GetListAllAsync().FirstOrDefault(a => a.Id == id)
                    ;
                    enableDish.Disable = !enableDish.Disable;
                    var r = await _manager.UpdateAsync(enableDish);
                }
                return Json("win");
            }
            catch (Exception ex)
            {
                return Json("lost");
            }
        }
        [Permission("Create")]
        [HttpGet]
        public virtual async Task<IActionResult> CreateAsync()
        {
            var data = new DishEditViewModel(null);
            return View(data);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync(DishEditViewModel dish)
        {
            {
                //dish.Photo = Encoding.ASCII.GetBytes(dish.PhotoBase64);
                //var (s, a) = await _manager.AddAsync(dish.ToModel());
                if (dish.PhotoUpload != null)
                {
                    using (var stream = dish.PhotoUpload.OpenReadStream())
                    using (var memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        dish.Photo = memStream.ToArray();

                        memStream.Close();
                        memStream.Dispose();
                        var (s, a) = await _manager.AddAsync(dish.ToModel());
                        return RedirectToAction("Details", new { id = a.Id });
                    }
                }
                else
                {
                    dish.CreatedDate = DateTime.Now;
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
                        ViewBag.Notices.Add(AlertModel.Error("Dish not found"));
                    }
                    else
                    {
                        ViewBag.Notice = AlertModel.Error(state.ToString());
                        return View();
                    }
                    return RedirectToAction("Details", new { id = viewItem.Id });
                }
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
                return View(new DishViewModel(viewItem));
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
            return View(new DishViewModel(default(Dish)));
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
                return View(new DishViewModel(ViewItem));
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
            return View(new DishViewModel(default(Dish)));
        }

        [HttpGet]
        [Permission("Delete")]
        public virtual async Task<IActionResult> DeletedAsync()
        {
            var topics = new DishViewModel(null);
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
                return View(new DishEditViewModel(viewItem));
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

            return View(new DishEditViewModel(default(Dish)));
        }

        [HttpPost]
        public virtual async Task<IActionResult> EditAsync(DishEditViewModel model)
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
