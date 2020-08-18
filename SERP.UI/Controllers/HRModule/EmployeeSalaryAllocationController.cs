using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.HRModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.HRModule
{
    public class EmployeeSalaryAllocationController : Controller
    {
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<EmployeeSalaryModel, int> _employeeSalaryRepo;
        private readonly IGenericRepository<PayHeadesModel, int> _payHeadRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public EmployeeSalaryAllocationController(IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<EmployeeSalaryModel, int> empSalaryRepo, IGenericRepository<PayHeadesModel, int> payHeadRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _employeeRepo = employeeRepo;
            _employeeSalaryRepo = empSalaryRepo;
            _payHeadRepo = payHeadRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var employeeDetail = await _employeeRepo.GetList(x => x.IsActive == 1);
                employeeDetail.ToList().ForEach(item =>
                {
                    item.Name = item.Name + " (" + item.EmpCode + " )";
                });
                ViewBag.employeeList = employeeDetail;
                return PartialView("~/Views/HRModule/_EmployeeSalaryAllocationPartial.cshtml");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        public async Task<IActionResult> GetEmployeeSalary(int empId)
        {
            try
            {
                List<EmployeeSalaryDetailModel> employeeSalaryDetailModels = new List<EmployeeSalaryDetailModel>();
                employeeSalaryDetailModels = (from ESI in await _employeeSalaryRepo.GetList(x => x.IsActive == 1 && x.EmployeeId == empId)
                                              join PM in await _payHeadRepo.GetList(x => x.IsActive == 1)
                                              on ESI.HeadId equals PM.Id
                                              select new EmployeeSalaryDetailModel
                                              {
                                                  HeadId = PM.Id,
                                                  HeadName = PM.Name,
                                                  Amount = ESI.Amount,
                                                  AdditionDeduction = PM.Addition_Deduction,
                                                  IsDependentOnDay = PM.IsDependentPerDay
                                              }).ToList();

                employeeSalaryDetailModels.ForEach(item =>
                {
                    item.EmployeeId = empId;
                });

                return PartialView("~/Views/HRModule/EmployeeSalaryPartial.cshtml", employeeSalaryDetailModels);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeSalary(string[] Amount, string[] HeadId, int employeeId)
        {
            try
            {
                List<EmployeeSalaryModel> salModels = new List<EmployeeSalaryModel>();
                var models = await _employeeSalaryRepo.GetList(x => x.EmployeeId == employeeId && x.IsActive == 1 && x.IsDeleted == 0);
                for (int i = 0; i < HeadId.Count(); i++)
                {
                    EmployeeSalaryModel model = new EmployeeSalaryModel();
                    model = models.ToList().Where(x => x.HeadId == Convert.ToInt32(HeadId[i])).FirstOrDefault();
                    model.Amount = Convert.ToDecimal(Amount[i]);
                    salModels.Add(model);

                }
                await _employeeSalaryRepo.CreateNewContext();
                var response = await _employeeSalaryRepo.Update(salModels.ToArray());
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }
    }
}