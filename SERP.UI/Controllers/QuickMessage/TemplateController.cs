using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.QuickMessage;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Controllers.QuickMessage
{
    public class TemplateController : Controller
    {
        private readonly IGenericRepository<MessageTemplate, int> _IMessageTemplateRepository;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        /// <summary>
        /// Inject the required service to the constructor
        /// </summary>
        /// <param name="messageTemplateRepo"></param>
        public TemplateController(IGenericRepository<MessageTemplate, int> messageTemplateRepo, IGenericRepository<ExceptionLogging, int>exceptionLogRepo)
        {
            _IMessageTemplateRepository = messageTemplateRepo;
            _exceptionLoggingRepo = exceptionLogRepo;
        }


        public async Task<IActionResult> MessageTemplate( int id)
        {
            var messageModel = await _IMessageTemplateRepository.GetSingle(x => x.Id == id);
            return PartialView("~/Views/QuickMessage/QuickMessageView.cshtml", messageModel);
        }

        public async Task<IActionResult> GetMessageTemplate()
        {
            try
            {
                return PartialView("~/Views/QuickMessage/MessageTemplateList.cshtml",
                    await _IMessageTemplateRepository.GetList(x => x.IsActive == 1));
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
        public async Task<IActionResult> CreateTemplate(MessageTemplate model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                if (model.Id > 0)
                {
                    var response = await _IMessageTemplateRepository.Update(CommanDeleteHelper.CommanUpdateCode<MessageTemplate>(model, 1));
                    return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(response));
                }
                return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IMessageTemplateRepository.CreateEntity(model)));
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
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<MessageTemplate>(await _IMessageTemplateRepository.GetSingle(x => x.Id == id), 1);
                await _IMessageTemplateRepository.CreateNewContext();
                return Json(Utilities.ResponseMessage.ResponseData.Instance.GenericResponse(await _IMessageTemplateRepository.Update(deleteModel)));
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
