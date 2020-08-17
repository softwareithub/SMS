using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;

namespace SERP.UI.Controllers.HRModule
{
    public class SalarySlipController : Controller
    {
        private readonly ISalaryHeadRepo _salaryHeadRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<InstituteMaster, int> _instituteRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public SalarySlipController(ISalaryHeadRepo salaryHeadRepo, 
                                    IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, 
                                    IGenericRepository<InstituteMaster, int>  instituteRepo,
                                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _salaryHeadRepo = salaryHeadRepo;
            _employeeRepo = employeeRepo;
            _instituteRepo = instituteRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.employeeList = await _employeeRepo.GetList(x => x.IsActive == 1);
                return await Task.Run(() => PartialView("~/Views/HRModule/_SalarySlipPartial.cshtml"));
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

        [HttpGet]
        public async Task<IActionResult> SalarySlipDetails(int employeeId, int year, int month)
        {
            try
            {
                var instituteModel = await _instituteRepo.GetSingle(x => x.IsActive == 1);
                List<EmployeeSalarySlip> models = new List<EmployeeSalarySlip>();


                var DateTime = new DateTime(year, month, 1);
                models = await _salaryHeadRepo.GetEmployeeSalarySlip(month, employeeId, DateTime);
                models.First().InstituteMaster = instituteModel;
                return PartialView("~/Views/HRModule/_EmployeeSalarySlipPartial.cshtml", models);
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

        public async Task<IActionResult> SalaryStatment()
        {
            try
            {
                var responseData = await _salaryHeadRepo.GetSalaryStatement(7, 31);
                return PartialView("~/Views/HRModule/_salaryStatement.cshtml", responseData);
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
    }
}