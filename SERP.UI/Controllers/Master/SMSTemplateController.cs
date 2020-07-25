using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;


namespace SERP.UI.Controllers.Master
{
    public class SMSTemplateController : Controller
    {
        private readonly IGenericRepository<SMSTemplateModel, int> _ISMSTemplateRepo;

        public SMSTemplateController(IGenericRepository<SMSTemplateModel, int> smsTemplateRepo)
        {
            _ISMSTemplateRepo = smsTemplateRepo;

        }
        public async Task<IActionResult> Create(int id)
        {
            return PartialView("~/Views/SMSTemplate/_CreateSMSTemplate.cshtml", await _ISMSTemplateRepo.GetSingle(x => x.Id == id));
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
            var models = await _ISMSTemplateRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/SMSTemplate/_SMSTemplateList.cshtml", models);
        }
    }
}