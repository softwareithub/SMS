using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class AcademicCalenderController : Controller
    {
        private readonly IGenericRepository<AcademicCalender, int> _IAcademicCalenderRepo;
        private readonly IGenericRepository<AcademicMaster, int> _academicMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public AcademicCalenderController(IGenericRepository<AcademicCalender, int> academiCalenderRepo, 
                                          IGenericRepository<AcademicMaster, int> academicMasterRepo,
                                          IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
                                          )
        {
            _IAcademicCalenderRepo = academiCalenderRepo;
            _academicMasterRepo = academicMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> AcademicCalender(int id)
        {
            try
            {
                ViewBag.AcademicList = await _academicMasterRepo.GetList(x => x.IsActive == 1);
                var responseData = await _IAcademicCalenderRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/AcademicCalender/_AcademicCalenderPartial.cshtml", responseData);
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
        public async Task<IActionResult> CreateAcademicCalender(AcademicCalender model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var response = await _IAcademicCalenderRepo.Update(CommanDeleteHelper.CommanUpdateCode<AcademicCalender>(model, 1));
                    return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(response));
                }
                return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IAcademicCalenderRepo.CreateEntity(model)));
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

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<AcademicCalender>(await _IAcademicCalenderRepo.GetSingle(x => x.Id == id), 1);
                await _IAcademicCalenderRepo.CreateNewContext();
                return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IAcademicCalenderRepo.Update(deleteModel)));
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

        public async Task<IActionResult> GetAcademicCalender()
        {
            try
            {
                return PartialView("~/Views/AcademicCalender/_AcademicCalenderListPartial.cshtml", await _IAcademicCalenderRepo.GetList(x => x.IsActive == 1));
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