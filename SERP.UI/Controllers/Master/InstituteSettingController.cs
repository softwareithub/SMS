using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.Master
{
    public class InstituteSettingController : Controller
    {
        private readonly IGenericRepository<InstituteSettingModel, int> _instituteRepo;
        private readonly IGenericRepository<InstituteMaster, int> _instituteMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public InstituteSettingController(IGenericRepository<InstituteSettingModel, int> instituteRepo, 
                                          IGenericRepository<InstituteMaster, int> instituteMasterRepo,
                                          IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _instituteRepo = instituteRepo;
            _instituteMasterRepo = instituteMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var instituteModel = (await _instituteMasterRepo.GetSingle(x => x.IsActive == 1)).Id;
                var model = await _instituteRepo.GetSingle(x => x.InstituteId == instituteModel);
                return PartialView("~/Views/InstituteMaster/_InstituteSettingPartial.cshtml", model);
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
        public async Task<IActionResult> Create(InstituteSettingModel model)
        {
            try
            {
                var instituteModel = (await _instituteMasterRepo.GetSingle(x => x.IsActive == 1)).Id;
                model.InstituteId = instituteModel;
                if (model.Id == 0)
                {
                    var response = await _instituteRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(response));
                }
                else
                {
                    var updateModel = CommanDeleteHelper.CommanUpdateCode(model, 1);
                    var response = await _instituteRepo.Update(updateModel);
                    return Json(ResponseData.Instance.GenericResponse(response));
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
    }
}