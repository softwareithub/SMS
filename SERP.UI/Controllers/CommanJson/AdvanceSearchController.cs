using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.CommanJson
{
    public class AdvanceSearchController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<BookMasterModel, int> _bookRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public AdvanceSearchController(IGenericRepository<StudentMaster, int> studentRepo,
                                       IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, 
                                       IGenericRepository<BookMasterModel, int> bookRepo,
                                       IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _studentRepo = studentRepo;
            _employeeRepo = employeeRepo;
            _bookRepo = bookRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetStudent()
        {
            try
            {
                List<string> studentList = new List<string>();
                var studentModels = await _studentRepo.GetList(x => x.IsActive == 1);
                studentModels.ToList().ForEach(item =>
                {
                    studentList.Add(item.Name + " (" + item.RegistrationNumber + ") ");
                });

                return Json(studentList);
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

        public async Task<IActionResult> GetEmployeeDetails()
        {
            try
            {
                List<string> employeeList = new List<string>();
                var employeeDetails = await _employeeRepo.GetList(x => x.IsActive == 1);
                employeeDetails.ToList().ForEach(item =>
                {
                    employeeList.Add(item.Name + " (" + item.EmpCode + ") ");
                });
                return Json(employeeList);
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

        public async Task<IActionResult> GetBookDetails()
        {
            try
            {
                var bookDetails = await _bookRepo.GetList(x => x.IsActive == 1);
                List<string> bookList = new List<string>();

                bookDetails.ToList().ForEach(item =>
                {
                    bookList.Add(item.TitleName + " (" + item.AuthorName + ") ");
                });

                return Json(bookList);
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