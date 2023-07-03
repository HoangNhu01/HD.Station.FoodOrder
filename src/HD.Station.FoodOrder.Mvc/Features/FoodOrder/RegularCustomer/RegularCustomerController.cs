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
    //[Area("FoodOrder")]
    //public class RegularCustomerController : Controller
    //{
    //    private readonly IRegularCustomerManager _manager;
    //    public RegularCustomerController(IRegularCustomerManager manager)
    //    {
    //        _manager = manager;

    //    }
    //    public async Task<IActionResult> IndexAsync(string filter, bool includeDisabled)
    //    {
    //        var model = new RegularCustomerIndexViewModel
    //        {
    //            Filter = filter,
    //        };
      
    //        var list = (await _customerManager.QueryAsync()).Select(o => new SelectListItem { Text = o.FullName, Value = o.Id.ToString() }).ToList();
    //        var createModel = new RegularCustomerEditViewModel() {
    //            Customers=list,
    //        };
            
    //        ViewBag.CreateModel = createModel;

    //        return View(model);
    //    }

    //    [Permission(nameof(IndexAsync))]
    //    public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
    //    {
    //        try
    //        {
    //            var start = DateTime.Now;
    //            var items = (await _manager.QueryAsync()).Include(a => a.Customer);

    //            var query = items.Where(a => string.IsNullOrEmpty(filterText) || a.Customer.FullName.Contains(filterText))
    //                                .OrderBy(a => a.Customer.FullName)
    //                                .AsNoTracking().Select(a => new RegularCustomerViewModel(a));
    //            var result = await query.ToDataSourceResultAsync(request);
    //            return Json(query);
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { ex.Message, ex.StackTrace });
    //        }
    //    }
    //    [HttpGet]
    //    public virtual async Task<IActionResult> GetModelForCustomerAsync(Guid idCustomer)
    //    {
    //        var rs = await _customerManager.QueryAsync();
    //        var md = rs.ToList().Where(m => m.Id == idCustomer);
    //        return Json(md);
    //    }

    //    [Permission("Create")]
    //    [HttpGet]
    //    public virtual async Task<IActionResult> CreateAsync()
    //    {
    //        var data = new RegularCustomerEditViewModel();
    //        data.Customers = (await _customerManager.QueryAsync()).Select(o => new SelectListItem { Text = o.FullName, Value = o.Id.ToString()}).ToList();
    //        return View(data);
    //    }

    //    [HttpPost]
    //    public virtual async Task<IActionResult> CreateAsync(RegularCustomerEditViewModel regularCustomer)
    //    {
    //        regularCustomer.CreatedDate = DateTime.Now;
    //        var check = await _manager.GetByCustomerIdAsync(regularCustomer.CustomerId);
    //        if (check == null)
    //        {
    //            var (state, viewItem) = await _manager.AddAsync(regularCustomer.ToModel());
    //            if (state.Succeeded)
    //            {
    //                if (viewItem == null)
    //                {
    //                    return NotFound();
    //                }
    //                return RedirectToAction("Details", new { id = viewItem.Id });
    //            }
    //            else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //            {
    //                ViewBag.Notices.Add(AlertModel.Error("RegularCustomer not found"));
    //            }
    //            else
    //            {
    //                ViewBag.Notice = AlertModel.Error(state.ToString());
    //                return View();
    //            }
    //            return RedirectToAction("Details", new { id = viewItem.Id });
    //        }
    //        return RedirectToAction("Create");
    //    }

    //    [HttpGet("[area]/[controller]/{id:guid}")]
    //    public virtual async Task<IActionResult> DetailsAsync(Guid id)
    //    {
    //        var viewModel = (await _manager.QueryAsync()).Where(a => a.Id == id).Include(a => a.Customer).FirstOrDefault();
    //        if (viewModel == null)
    //        {
    //            return NotFound();
    //        }
    //        var model = new RegularCustomerViewModel(viewModel);
    //        return View(model);
    //        //var (state, viewItem) = await _manager.ReadByIdAsync(id);
    //        //ViewBag.Id = id;
    //        //if (state.Succeeded)
    //        //{
    //        //    if (viewItem == null)
    //        //    {
    //        //        return NotFound();
    //        //    }
    //        //    var model = new RegularCustomerViewModel(viewItem);
    //        //    return View(model);
    //        //}
    //        //else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        //{
    //        //    ViewBag.Notices.Add(AlertModel.Error("RegularCustomer not found"));
    //        //}
    //        //else
    //        //{
    //        //    ViewBag.Notice = AlertModel.Error(state.ToString());
    //        //    return View();
    //        //}
    //        //return View(new RegularCustomerViewModel(default(RegularCustomer)));
    //    }

    //    [HttpGet]
    //    [Permission("Delete")]
    //    public virtual async Task<IActionResult> DeleteAsync(Guid id, string message, string urlReturn)
    //    {
    //        ViewBag.Id = id;
    //        var listNotice = new List<NoticeModel>();
    //        if (!string.IsNullOrWhiteSpace(message))
    //        {
    //            listNotice.Add(JsonConvert.DeserializeObject<AlertModel>(message));
    //        }
    //        ViewBag.Notices = listNotice;
    //        ViewData["ReturnUrl"] = urlReturn;
    //        var (State, ViewItem) = await _manager.DeletePreCheckAsync(id);
    //        if (State.Succeeded)
    //        {
    //            if (ViewItem == null)
    //            {
    //                return NotFound();
    //            }
    //            if (State.Message != null)
    //            {
    //                ViewBag.Notices.Add(AlertModel.Warning(State.ToString()));
    //            }
    //            return View(new RegularCustomerViewModel(ViewItem));
    //        }
    //        else if (State.Exception != null && State.Exception is UnauthorizedAccessException)
    //        {
    //            return Unauthorized();
    //        }
    //        else if (ViewItem == null)
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            ViewBag.Notices.Add(AlertModel.Error(State.ToString()));
    //        }
    //        return View(new RegularCustomerViewModel(default(RegularCustomer)));
    //    }

    //    [HttpGet]
    //    [Permission("Delete")]
    //    public virtual async Task<IActionResult> DeletedAsync()
    //    {
    //        var topics = new RegularCustomerViewModel();
    //        ViewBag.Notice = new AlertModel()
    //        {
    //            Contextual = "success",
    //            Title = "Successed",
    //            Message = "Deleted!"
    //        };
    //        return View(topics);
    //    }

    //    [HttpPost]
    //    [Permission("Delete")]
    //    public virtual async Task<IActionResult> DeleteConfirmedAsync([FromRoute] Guid id)
    //    {
    //        var stateAnother = await _manager.DeleteInAnotherRecordAsync(id);
    //        var state = await _manager.DeleteByIdAsync(id);
    //        if (state.Succeeded)
    //        {
    //            return RedirectToAction("Deleted");
    //        }
    //        else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        {
    //            return Unauthorized();
    //        }
    //        return RedirectToAction("Delete", new { id = id, message = JsonConvert.SerializeObject(state.ToAlert()) });
    //    }
    //    [Permission("Edit")]
    //    [HttpGet]
    //    public virtual async Task<IActionResult> EditAsync(Guid id)
    //    {

    //        var (state, viewItem) = await _manager.ReadByIdAsync(id);

    //        if (state.Succeeded)
    //        {
    //            if (viewItem == null)
    //            {
    //                return NotFound();
    //            }
    //            return View(new RegularCustomerEditViewModel(viewItem));
    //        }
    //        else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        {
    //            ViewBag.Notices.Add(AlertModel.Error("RegularCustomer not found"));
    //        }
    //        else
    //        {
    //            ViewBag.Notice = AlertModel.Error(state.ToString());
    //            return View();
    //        }

    //        return View(new RegularCustomerEditViewModel(default(RegularCustomer)));
    //    }

    //    [HttpPost]
    //    public virtual async Task<IActionResult> EditAsync(RegularCustomerEditViewModel model)
    //    {
    //        var vm = model.ToModel();
    //        var state = await _manager.UpdateAsync(vm);
    //        if (state.Succeeded)
    //        {
    //            ViewBag.Notice = new AlertModel()
    //            {
    //                Contextual = "success",
    //                Title = "Thành công",
    //                Message = "Đã sửa thành công!"
    //            };

    //            return RedirectToAction("Details", new { Id = vm.Id });
    //        }
    //        ViewBag.Notice = state.ToAlert();
    //        return View(model);
    //    }
    //}
}
