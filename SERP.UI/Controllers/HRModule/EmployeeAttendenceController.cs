using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class EmployeeAttendenceController : Controller
    {
        private readonly IGenericRepository<EmployeeAttendenceModel, int> _employeeAttendenceRepo;
        private readonly IGenericRepository<DepartmentModel, int> _departmentRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        public EmployeeAttendenceController(IGenericRepository<EmployeeAttendenceModel, int> attendRepo,
            IGenericRepository<DepartmentModel, int> departRepo, IGenericRepository<EmployeeBasicInfoModel, int> basicInfoRepo)
        {
            _employeeAttendenceRepo = attendRepo;
            _departmentRepo = departRepo;
            _basicInfoRepo = basicInfoRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Departments = await _departmentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/HRModule/_EmployeeAttendenceIndex.cshtml");
        }

        public async Task<IActionResult> GetEmployeeList(int id, string attDate)
        {
            DateTime dt = DateTime.Parse(attDate);
            List<EmployeeAttendenceVm> models = new List<EmployeeAttendenceVm>();
            var employees = await _basicInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.DepartmentId == id);

            employees.ToList().ForEach(item =>
            {
                EmployeeAttendenceVm model = new EmployeeAttendenceVm();
                model.EmployeeId = item.Id;
                model.EmployeeCode = item.EmpCode;
                model.EmployeeName = item.Name;
                model.Image = item.Photo;
                models.Add(model);
            });

            var employeeAttendences = await _employeeAttendenceRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.AttendenceDate == dt.Date);

            models.ForEach(model =>
            {
                employeeAttendences.ToList().ForEach(item =>
                {
                    if (model.EmployeeId == item.EmployeeId)
                    {
                        model.AttendenceType = item.AttendenceType;
                    }
                });
            });
            return PartialView("~/Views/HRModule/_EmployeeAttendencePartial.cshtml", models);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAttendence(EmployeeAttendenceVm model)
        {
            if (model.LongLeaveType == "S")
            {
                return await CreateShortAttendence(model);
            }
            else
            {
                return await CreateLongVacationLeave(model);
            }
        }

        private async Task<IActionResult> CreateLongVacationLeave(EmployeeAttendenceVm model)
        {
            var employeeIds = Request.Form["empId"];
            var AttendType = Request.Form["AttendType"];

            List<DateTime> allDates = new List<DateTime>();
            for (DateTime date = model.AttendenceDate; date <= model.ToAttendenceDate; date = date.AddDays(1))
                allDates.Add(date);

            var models = await _employeeAttendenceRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

            var employees = models.Where(item => allDates.Contains(item.AttendenceDate) &&
              (Array.ConvertAll<string, int>(employeeIds, new Converter<string, int>(Convert.ToInt32))).ToList().Contains(item.EmployeeId));

            employees.ToList().ForEach(x =>
            {
                x.IsActive = 0;
                x.IsDeleted = 1;
                x.UpdatedBy = 1;
                x.UpdatedDate = DateTime.Now.Date;
            });
            await _employeeAttendenceRepo.CreateNewContext();
            var deleteResult = await _employeeAttendenceRepo.Delete(employees.ToArray());
            await _employeeAttendenceRepo.CreateNewContext();

            List<EmployeeAttendenceModel> empAttendenceModels = new List<EmployeeAttendenceModel>();

            allDates.ForEach(date =>
            {
                for (int i = 0; i < employeeIds.Count(); i++)
                {
                    EmployeeAttendenceModel attmodel = new EmployeeAttendenceModel()
                    {
                        AttendenceDate = date,
                        AttendenceType = model.LongLeaveType,
                        EmployeeId = Convert.ToInt32(employeeIds[i]),
                    };
                    empAttendenceModels.Add(attmodel);
                }
            });

            var result = await _employeeAttendenceRepo.Add(empAttendenceModels.ToArray());
            return Json(ResponseData.Instance.GenericResponse(result));
        }

        private async Task<IActionResult> CreateShortAttendence(EmployeeAttendenceVm model)
        {
            var employeeIds = Request.Form["empId"];
            var AttendType = Request.Form["AttendType"];

            List<EmployeeAttendenceModel> models = new List<EmployeeAttendenceModel>();

            await DeleteEmployeeAttendence((Array.ConvertAll<string, int>(employeeIds, new Converter<string, int>(Convert.ToInt32))).ToList(), model.AttendenceDate.Date);

            for (int i = 0; i < employeeIds.Count(); i++)
            {
                EmployeeAttendenceModel dataModel = new EmployeeAttendenceModel();
                dataModel.AttendenceDate = model.AttendenceDate;
                dataModel.EmployeeId = Convert.ToInt32(employeeIds[i]);
                dataModel.AttendenceType = AttendType[i].ToString();
                models.Add(dataModel);
            }

            await _employeeAttendenceRepo.CreateNewContext();
            var result = await _employeeAttendenceRepo.Add(models.ToArray());
            return Json(ResponseData.Instance.GenericResponse(result));
        }

        private async Task DeleteEmployeeAttendence(List<int> employeeIds, DateTime attendenceDate)
        {

            var model = await _employeeAttendenceRepo.GetList(x => x.AttendenceDate == attendenceDate
                                    && x.IsActive == 1 && x.IsDeleted == 0);

            model.ToList().ForEach(item =>
            {
                employeeIds.ForEach(data =>
                {
                    if (item.EmployeeId == data)
                    {
                        item.IsDeleted = 1;
                        item.IsActive = 0;
                        item.UpdatedBy = 1;
                        item.UpdatedDate = DateTime.Now.Date;
                    }
                });
            });

            await _employeeAttendenceRepo.CreateNewContext();

            var result = await _employeeAttendenceRepo.Delete(model.ToArray());
        }
    }
}