using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.ExamMaster
{
    public class ExamController : Controller
    {
        private readonly IGenericRepository<Exam, int> _examRepo;

        public ExamController(IGenericRepository<Exam, int> examRepo)
        {
            _examRepo = examRepo;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var result = await _examRepo.GetSingle(x => x.Id == Id);
            return await Task.Run(()=> PartialView("~/Views/ExamMaster/_ExamPartial.cshtml",result));
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam(Exam model)
        {
            if (model.Id == 0)
            {
                var result = await _examRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                var result = await _examRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }

        }

        public async Task<IActionResult> GetExamList()
        {
            var result = await _examRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/ExamMaster/_ExamListPartial.cshtml", result);
        }

        public async Task<IActionResult> DeleteExam(int Id)
        {
            var model = await _examRepo.GetSingle(x => x.Id == Id);
            model.IsActive = 0;
            model.IsDeleted = 1;
            model.UpdatedBy = 1;
            model.UpdatedDate = DateTime.Now.Date;
            await _examRepo.CreateNewContext();
            var result = await _examRepo.Update(model);
            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}