using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Calender;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.DashBoardModel;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.DashBoard
{
    public class DashBoardMasterController : Controller
    {
        private readonly IDashBoardGraphRepo _IDashBoardRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        private readonly IGenericRepository<AcademicCalender, int> _IAcademicCalenderRepo;
        public DashBoardMasterController(IDashBoardGraphRepo dashBoardRepo,
                                         IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo,
                                         IGenericRepository<AcademicCalender, int> academicCalender
                                            )
        {
            _IDashBoardRepo = dashBoardRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
            _IAcademicCalenderRepo = academicCalender;
        }
        public async Task<IActionResult> GetStudentCourseBatchStrenght()
        {
            try
            {
                var result = await _IDashBoardRepo.BatchCourseCount();
                return Json(result);
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

        public async Task<IActionResult> StudentAttendeceReport()
        {
            try
            {
                var result = await _IDashBoardRepo.GetRootList<DateTime>(DateTime.Now.Date);
                return await Task.Run(() => Json(result));
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

        public async Task<IActionResult> StudentAttendeceReportMonthDayWise(int year, int month, int day)
        {
            try
            {
                year = year == 0 ? DateTime.Now.Year : year;
                month = month == 0 ? DateTime.Now.Month : month;
                day = day == 0 ? DateTime.Now.Day : day;
                var result = await _IDashBoardRepo.GetStudentAttendenceReport(year, month, day);
                return await Task.Run(() => Json(result));
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

        public async Task<IActionResult> MenuSubMenuDetails()
        {
            try
            {
                var menuSubMenuDetails = HttpContext.Session.GetObject<List<MenuSubMenuVm>>("menuSubMenu");
                return await Task.Run(() => PartialView("~/Views/DashBoard/_MenuSubMenuDetails.cshtml", menuSubMenuDetails));
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
        public async Task<IActionResult> GetMenuDescription()
        {
            try
            {
                return await Task.Run(() => PartialView("~/Views/Shared/_SubMenuDescriptionPartial.cshtml"));
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

        public async Task<IActionResult> GetFeeDetailReport()
        {
            try
            {
                var result = await _IDashBoardRepo.GetFeeDetails();
                return await Task.Run(() => Json(result));
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

        public async Task<IActionResult> GetStudentOnlineTestResult(int userId)
        {
            try
            {
                var result = await _IDashBoardRepo.GetStudentOnlineTestResult(userId);
                return await Task.Run(() => Json(result));
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

        public async Task<IActionResult> InstituteCalender()
        {
            return await Task.Run(() => PartialView("~/Views/DashBoard/_InstituteCalenderPartial.cshtml"));
        }

        public async Task<IActionResult> GetCalenderDetails()
        {
            var calenderModels = await _IAcademicCalenderRepo.GetList(x => x.IsActive == 1);
            var models = new List<CIrcularCalender>();
            calenderModels.ToList().ForEach(data =>
            {
                var model = new CIrcularCalender();
                model.StartDate = data.FromDate;
                model.EndDate = data.ToDate;
                model.Title = data.EventName;
                model.Description = data.EventDescription;
                models.Add(model);

            });
            return Json(models);

        }


        public async Task<IActionResult> GetFeeDetailMonthWise()
        {
            var responseModels = await _IDashBoardRepo.YearWiseFeeDetails();
            return PartialView("~/Views/DashBoard/YearWiseFeeDetailRepository.cshtml", responseModels);

        }

        public async Task<IActionResult> DefaulterList(string month, int year)
        {
            var responseModels = await _IDashBoardRepo.DefaulterList(month, year);
            return PartialView("~/Views/DashBoard/DefaulterPartial.cshtml", responseModels);

        }

        public async Task<IActionResult> SendWhatsAppMessageC(string message)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.ultramsg.com/instance11520/messages/chat"))
                {
                    var contentList = new List<string>();
                    contentList.Add("token=216tq4ry5y9cbhek");
                    contentList.Add("to=+919315775084");
                    contentList.Add("body=" + message + "");
                    contentList.Add("priority=10");
                    contentList.Add("referenceId=");
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
                }
            }

            return Json("Message Sent !");
        }
    }
}