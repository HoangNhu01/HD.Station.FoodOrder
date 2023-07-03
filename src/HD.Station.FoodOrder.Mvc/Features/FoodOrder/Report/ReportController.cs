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

namespace HD.Station.FoodOrder
{
    //[Area("FoodOrder")]
    //public class ReportController : Controller
    //{
    //    private readonly IReportManager _manager;
    //    private readonly ICustomerManager _customerManager;
    //    private readonly IReportCustomerManager _reportCustomerManager;
    //    public ReportController(IReportManager manager, ICustomerManager customerManager, IReportCustomerManager reportCustomerManager)
    //    {
    //        _manager = manager;
    //        _customerManager = customerManager;
    //        _reportCustomerManager = reportCustomerManager;
    //    }
    //    public IActionResult Index(string filter, bool includeDisabled)
    //    {
    //        var model = new ReportIndexViewModel
    //        {
    //            Filter = filter,
    //        };
    //        return View(model);
    //    }

    //    [Permission(nameof(Index))]
    //    public virtual async Task<IActionResult> ReadAsync([DataSourceRequest] DataSourceRequest request, [FromForm] string filterText)
    //    {
    //        try
    //        {
    //            var start = DateTime.Now;
    //            var items = (await _manager.QueryAsync()).Include(a => a.ReportCustomers);
    //            var customers = new List<Customer>();
    //            var cusName = "";
    //            foreach (var item in items)
    //            {
    //                var repCus = (await _reportCustomerManager.QueryAsync()).Where(a => a.ReportId == item.Id);

    //                //dishes.AddRange(mealMenus.Dishes);
    //                foreach (var rep in repCus)
    //                {
    //                    customers.Add(rep.Customer);
    //                    var d = (await _customerManager.QueryAsync()).FirstOrDefault(a => a.Id == rep.CustomerId);
    //                    cusName += d.FullName + "; ";
    //                }
    //            }

    //            var query = items.Where(a => string.IsNullOrEmpty(filterText))
    //                                .OrderBy(a => a.CreatedDate)
    //                                .AsNoTracking().Select(a => new ReportViewModel(a)
    //                                {
    //                                    Customers = customers,
    //                                    CustomerNames = cusName
    //                                });
    //            var result = await query.ToDataSourceResultAsync(request);
    //            return Json(result);
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { ex.Message, ex.StackTrace });
    //        }
    //    }

    //    [Permission("Create")]
    //    [HttpGet]
    //    public virtual async Task<IActionResult> CreateAsync()
    //    {
    //        var items = await _manager.GetListAllAsync();
    //        var customers = _customerManager.GetListAllAsync();
    //        ViewBag.Customers = customers.Select(a => new SelectListItem { Text = a.FullName, Value = a.Id.ToString() }).ToList();
    //        var data = new ReportEditViewModel(null);
    //        return View(data);
    //    }

    //    [HttpPost]
    //    public virtual async Task<IActionResult> CreateAsync(ReportEditViewModel report)
    //    {
    //        report.CreatedDate = DateTime.Now;
    //        var (state, viewItem) = await _manager.AddAsync(report.ToModel());
    //        if (state.Succeeded)
    //        {
    //            if (viewItem == null)
    //            {
    //                return NotFound();
    //            }
    //            return RedirectToAction("Details", new { id = viewItem.Id });
    //        }
    //        else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        {
    //            ViewBag.Notices.Add(AlertModel.Error("Report not found"));
    //        }
    //        else
    //        {
    //            ViewBag.Notice = AlertModel.Error(state.ToString());
    //            return View();
    //        }
    //        return RedirectToAction("Details", new { id = viewItem.Id });
    //    }

    //    [HttpGet("[area]/[controller]/{id:guid}")]
    //    public virtual async Task<IActionResult> DetailsAsync(Guid id)
    //    {
    //        var (state, viewItem) = await _manager.ReadByIdAsync(id);
    //        ViewBag.Id = id;
    //        if (state.Succeeded)
    //        {
    //            if (viewItem == null)
    //            {
    //                return NotFound();
    //            }
    //            return View(new ReportViewModel(viewItem));
    //        }
    //        else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        {
    //            ViewBag.Notices.Add(AlertModel.Error("Customer not found"));
    //        }
    //        else
    //        {
    //            ViewBag.Notice = AlertModel.Error(state.ToString());
    //            return View();
    //        }
    //        return View(new ReportViewModel(default(Report)));
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
    //            return View(new ReportViewModel(ViewItem));
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
    //        return View(new ReportViewModel(default(Report)));
    //    }

    //    [HttpGet]
    //    [Permission("Delete")]
    //    public virtual async Task<IActionResult> DeletedAsync()
    //    {
    //        var topics = new ReportViewModel();
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
    //            return View(new ReportEditViewModel(viewItem));
    //        }
    //        else if (state.Exception != null && state.Exception is UnauthorizedAccessException)
    //        {
    //            ViewBag.Notices.Add(AlertModel.Error("Report not found"));
    //        }
    //        else
    //        {
    //            ViewBag.Notice = AlertModel.Error(state.ToString());
    //            return View();
    //        }

    //        return View(new ReportEditViewModel(default(Report)));
    //    }

    //    [HttpPost]
    //    public virtual async Task<IActionResult> EditAsync(ReportEditViewModel model)
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
