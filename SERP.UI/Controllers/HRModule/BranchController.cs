using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class BranchController : Controller
    {
        private readonly IGenericRepository<BranchInfoModel, int> _branchRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public BranchController(IGenericRepository<BranchInfoModel, int> branchRepo,
            IHostingEnvironment hostingEnvironment,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
            )
        {
            _branchRepo = branchRepo;
            _hostingEnvironment = hostingEnvironment;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var result = await _branchRepo.GetSingle(x => x.Id == id);
                return View("~/Views/HRModule/_BranchCreatePartial.cshtml", result);
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
        public async Task<IActionResult> Create(BranchInfoModel model, IFormFile BranchLogo)
        {
            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                formFiles.Add(BranchLogo);

                var imagePaths = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnvironment);

                if (imagePaths.Count() == 0)
                {
                    model.BranchLogo = string.Empty;
                    model.BranchLogo = string.Empty;
                }
                else
                {
                    model.BranchLogo = string.IsNullOrEmpty(model.BranchLogo) ? imagePaths[0] : model.BranchLogo;
                    model.BranchLogo = string.IsNullOrEmpty(model.BranchLogo) ? imagePaths[1] : model.BranchLogo;
                }

                if (model.Id > 0)
                {
                    var result = await _branchRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    var result = await _branchRepo.CreateEntity(model);
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
        public async Task<IActionResult> GetBranchList()
        {
            try
            {
                var result = await _branchRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return PartialView("~/Views/HRModule/_BranchMasterList.cshtml", result);
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
        public async Task<IActionResult> GetBranchInfo(int id)
        {
            try
            {
                var result = await _branchRepo.GetSingle(x => x.Id == id);
                //To Do Create the More Info page like student info
                return PartialView("", result);
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
                var model = await _branchRepo.GetSingle(x => x.Id == id);
                model.IsActive = 0;
                model.IsDeleted = 1;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;

                await _branchRepo.CreateNewContext();

                var result = await _branchRepo.Delete(model);
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