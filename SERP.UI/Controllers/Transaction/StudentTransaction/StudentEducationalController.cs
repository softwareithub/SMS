using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transaction.StudentTransaction
{
    public class StudentEducationalController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IGenericRepository<StudentEducationalDetail, int> _educationalRepo;

        public StudentEducationalController(IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<StudentEducationalDetail, int> educationalRepo)
        {
            _studentRepo = studentRepo;
            _educationalRepo = educationalRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.StudentList = await _studentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var educationDetail = await _educationalRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/StudentMaster/_StudentEducationalPartial.cshtml",educationDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentEducationalDetail model)
        {
            if (model.Id == 0)
            {
                var response = await _educationalRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<StudentEducationalDetail>(model, 1);
                var response = await _educationalRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }
        public async Task<IActionResult> EducationDetails()
        {
            var models = (from SM in await _studentRepo.GetList(x => x.IsActive == 1)
                          join ED in await _educationalRepo.GetList(x => x.IsActive == 1)
                          on SM.Id equals ED.StudentId
                          select new StudentEducationalDetail
                          {
                              Id= ED.Id,
                              StudentName= SM.Name +" ("+SM.RegistrationNumber+") ",
                              University= ED.University,
                              CollegeName= ED.CollegeName,
                              CourseName= ED.CourseName,
                              YearOfJoining= ED.YearOfJoining,
                              YearOfPassing= ED.YearOfPassing,
                              EnrollmentNumber= ED.EnrollmentNumber
                          }).ToList();

            return PartialView("~/Views/StudentMaster/_StudentEducationDetailPartial.cshtml", models);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _educationalRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<StudentEducationalDetail>(model, 1);
            await _educationalRepo.CreateNewContext();
            var response = await _educationalRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}