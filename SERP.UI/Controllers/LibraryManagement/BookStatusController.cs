using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.LibraryManagement;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.LibraryManagement
{
    public class BookStatusController : Controller
    {
        private readonly IGenericRepository<BookStatusModel, int> _bookStatusRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public BookStatusController(IGenericRepository<BookStatusModel, int> bookStatusRepo,
                                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _bookStatusRepo = bookStatusRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = await _bookStatusRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/LibraryManagement/_BookStatusPartial.cshtml", model);
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
        public async Task<IActionResult> CreateStatus(BookStatusModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode<BookStatusModel>(model, 1);
                    return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.Update(updateModel)));
                }
                return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.CreateEntity(model)));
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

        public async Task<IActionResult> StatusList()
        {
            try
            {
                return PartialView("~/Views/LibraryManagement/BookStatusList.cshtml", await _bookStatusRepo.GetList(x => x.IsActive == 1));
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

        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                var model = await _bookStatusRepo.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<BookStatusModel>(model, 1);
                await _bookStatusRepo.CreateNewContext();
                return Json(ResponseData.Instance.GenericResponse(await _bookStatusRepo.Update(deleteModel)));
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