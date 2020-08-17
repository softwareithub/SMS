using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;


namespace SERP.UI.Controllers.Master
{
    public class SMSTemplateController : Controller
    {
        private readonly IGenericRepository<SMSTemplateModel, int> _ISMSTemplateRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public SMSTemplateController(IGenericRepository<SMSTemplateModel, int> smsTemplateRepo,
                                     IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _ISMSTemplateRepo = smsTemplateRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;

        }
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                return PartialView("~/Views/SMSTemplate/_CreateSMSTemplate.cshtml", await _ISMSTemplateRepo.GetSingle(x => x.Id == id));
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
        public async Task<IActionResult> Create(SMSTemplateModel model)
        {
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var response = await _ISMSTemplateRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _ISMSTemplateRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _ISMSTemplateRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode(model, 1);
            await _ISMSTemplateRepo.CreateNewContext();
            var response = await _ISMSTemplateRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> GetSMSTemplateList()
        {
            try
            {
                var models = await _ISMSTemplateRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/SMSTemplate/_SMSTemplateList.cshtml", models);
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