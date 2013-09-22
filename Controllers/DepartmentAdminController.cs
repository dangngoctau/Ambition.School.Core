using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ambition.School.Core.Extensions;
using Ambition.School.Core.Models;
using Ambition.School.Core.Services;
using Ambition.School.Core.ViewModels;
using Omu.ValueInjecter;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;

namespace Ambition.School.Core.Controllers
{
    [Admin]
    public class DepartmentAdminController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IOrchardServices _orchardServices;
        public Localizer T { get; set; }
        public DepartmentAdminController(IDepartmentService departmentService, IOrchardServices orchardServices)
        {
            _departmentService = departmentService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        #region Position

        public ActionResult CreatePosition()
        {
            return View();
        }

        [HttpPost, ActionName("CreatePosition")]
        public ActionResult CreatePositionPost(PositionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //perform creating new position
                var record = model.CreateRecord();
                _departmentService.CreatePosition(record);
                _orchardServices.Notifier.Add(NotifyType.Information, T("The position information has been created."));
                return RedirectToAction("ListPositions");
            }
            return View(model);
        }

        public ActionResult ListPositions()
        {
            return View();
        }

        #endregion

        #region Department

        public ActionResult CreateDepartment()
        {
            var viewModel = new DepartmentCreateViewModel();
            viewModel.UpdateViewModel(_departmentService.ListDepartments(), _departmentService.ListPositions());
            return View(viewModel);
        }

        [HttpPost, ActionName("CreateDepartment")]
        public ActionResult CreateDepartmentPost(DepartmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = new DepartmentRecord { Name = model.Name };
                //create parent department.
                if (model.ParentDepartmentId.HasValue)
                {
                    department.ParentDepartmentRecord = _departmentService.GetDepartment(model.ParentDepartmentId.Value);
                }
                //create postions.
                foreach (var item in model.AvailablePositions.Where(c => c.IsSelected).ToList())
                {
                    department.PositionDepartmentRecords.Add(new PositionsDepartmentRecord { DepartmentRecord = department, PositionRecord = new PositionRecord { Id = item.Id } });
                }
                _departmentService.CreateDepartment(department);
                _orchardServices.Notifier.Add(NotifyType.Information, T("The department information has been created."));
                return RedirectToAction("EditDepartment", new { id = department.Id });
            }
            model.UpdateViewModel(_departmentService.ListDepartments(), _departmentService.ListPositions());
            return View(model);
        }

        public ActionResult ListDepartments()
        {
            return View();
        }

        public ActionResult EditDepartment(int id)
        {
            var departmentRecord = _departmentService.GetDepartment(id);
            if (departmentRecord == null)
            {
                return new HttpNotFoundResult();
            }
            var editModel = departmentRecord.CreateEditModel(_departmentService.ListDepartments(new[] { id }), _departmentService.ListPositions());
            return View(editModel);
        }

        [HttpPost, ActionName("EditDepartment")]
        public ActionResult EditDepartmentPost(DepartmentEditViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                var department = _departmentService.GetDepartment(editModel.Id);
                //update name.
                department.InjectFrom(editModel, new object[] { "Name" });
                //update parent department.
                department.ParentDepartmentRecord = editModel.ParentDepartmentId.HasValue ? new DepartmentRecord { Id = editModel.ParentDepartmentId.Value } : null;
                //updating positions consists of 2 steps.
                //step 1. Delete unselected positions.
                var newLookups = editModel.AvailablePositions.Where(c => c.IsSelected).Select(c => c.Id).ToDictionary(d => d, d => false);
                var positionDepartmentRecords = department.PositionDepartmentRecords.ToReadOnlyCollection();
                foreach (var record in positionDepartmentRecords)
                {
                    if (newLookups.ContainsKey(record.PositionRecord.Id))
                    {
                        newLookups[record.PositionRecord.Id] = true;
                    }
                    else
                    {
                        var item = department.PositionDepartmentRecords.Single(c => c.Id == record.Id);
                        department.PositionDepartmentRecords.Remove(item);
                    }
                }
                //step 2. Add new positions.
                foreach (var item in newLookups.Where(c => !c.Value))
                {
                    department.PositionDepartmentRecords.Add(new PositionsDepartmentRecord { DepartmentRecord = department, PositionRecord = new PositionRecord { Id = item.Key } });
                }
                _departmentService.UpdateDepartment(department);
                _orchardServices.Notifier.Add(NotifyType.Information, T("The department information has been updated."));
                return RedirectToAction("ListDepartments");
            }
            editModel.UpdateViewModel(_departmentService.ListDepartments(), _departmentService.ListPositions());

            return View(editModel);
        }

        public JsonResult ListDepartmentsAjax()
        {
            var departments = _departmentService.ListDepartments();
            var result = new List<dynamic>();
            foreach (var department in departments)
            {
                var dept = new { di = department.Id, dn = department.Name, pds = new List<dynamic>() };
                foreach (var position in department.PositionDepartmentRecords)
                {
                    dept.pds.Add(new { pdi = position.Id, pn = position.PositionRecord.Name });
                }
                result.Add(dept);
            }

            //var positions = (from departmentRecord in departments
            //                 from item in departmentRecord.PositionDepartmentRecords
            //                 select new { i = item.Id, d = departmentRecord.Id, p = item.PositionRecord.Name }).ToList();
            //return new JsonResult
            //{
            //    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
            //    Data = new { departments = departments.Select(c => new { i = c.Id, n = c.Name }), positionDepartments = positions }
            //};
            return new JsonResult
            {
                Data = new { depts = result }
            };
        }

        #endregion
    }
}