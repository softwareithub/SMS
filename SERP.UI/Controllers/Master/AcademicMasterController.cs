using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using SERP.Utilities.ResponseMessage;
using System.Threading;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class AcademicMasterController : Controller
    {
        private readonly IGenericRepository<AcademicMaster, int> _IGenericRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public AcademicMasterController(IGenericRepository<AcademicMaster, int>  genericRepository,
                                        IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IGenericRepo = genericRepository;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/AcademicMaster/Index.cshtml"));

                var result = await _IGenericRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/AcademicMaster/Index.cshtml", result);
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
        public async Task<IActionResult> Create(AcademicMaster model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = 1;
                    var result = await _IGenericRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _IGenericRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
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

        [HttpGet]
        public async Task<IActionResult> GetAcademicList()
        {
            try
            {
                var result = await _IGenericRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/AcademicMaster/_AcademicList.cshtml", result);
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
        public async Task<IActionResult> DeleteRecord(int id)
        {
            try
            {
                Thread.Sleep(3000);
                var academicModel = await _IGenericRepo.GetSingle(x => x.Id == id);
                academicModel.IsActive = 0;
                academicModel.IsDeleted = 0;

                await _IGenericRepo.CreateNewContext();

                var result = await _IGenericRepo.Delete(academicModel);
                return Json(ResponseData.Instance.GenericResponse(result));
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