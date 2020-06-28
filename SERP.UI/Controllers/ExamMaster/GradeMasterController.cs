using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.ExamMaster
{
    public class GradeMasterController : Controller
    {
        private readonly IGenericRepository<GradeMaster, int> _IGradeMasterRepo;

        public GradeMasterController(IGenericRepository<GradeMaster, int> _gradeMasterRepo)
        {
            _IGradeMasterRepo = _gradeMasterRepo;
        }
        public async Task<IActionResult> GradeList()
        {
            var result = await _IGradeMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/ExamMaster/_GradeMasterPartial.cshtml",result);
        }

        public async Task<IActionResult> CreateGrade(int id )
        {
            var response = await _IGradeMasterRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/ExamMaster/_CreateGradeMasterPartial.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrade(GradeMaster model)
        {
            if (model.Id > 0)
            {
                model.IsActive = 1;
                model.IsDeleted = 0;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now;
                var response = await _IGradeMasterRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));

            }
            else
            {
                var response = await _IGradeMasterRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var response = await _IGradeMasterRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<GradeMaster>(response,1);
            await _IGradeMasterRepo.CreateNewContext();
            var responseData = await _IGradeMasterRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(responseData));

        }
    }

}