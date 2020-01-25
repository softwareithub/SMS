using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Master
{
    public class CourseController : Controller
    {
        private readonly IGenericRepository<CourseMaster, int> _IGenericRepo;
        public CourseController(IGenericRepository<CourseMaster, int> genericRepository)
        {
            _IGenericRepo = genericRepository;
        }
        public async Task<IActionResult> Index(int id = 0)
        {
            if (id == 0)
                return await Task.Run(() => PartialView("~/Views/CourseMaster/_CourseMasterPartial.cshtml"));

            var courseDetail = await _IGenericRepo.GetSingle(x => x.Id == id);
            var attendenceType = courseDetail.AttendenceType.Trim();
            courseDetail.AttendenceType = attendenceType;
            return PartialView("~/Views/CourseMaster/_CourseMasterPartial.cshtml", courseDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseMaster model)
        {
            if (model.Id == 0)
            {
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now.Date;
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

        [HttpGet]
        public async Task<IActionResult> GetCourseList()
        {
            var result = await _IGenericRepo.GetList(c => c.IsActive == 1 && c.IsDeleted == 0);
            return PartialView(PartialPages.PartialPageDetails.ClassDetailPartial, result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            var course = await _IGenericRepo.GetSingle(x => x.Id == Id);
            course.IsActive = 0;
            course.IsDeleted = 1;
            course.UpdatedBy = 1;
            course.UpdatedDate = DateTime.Now.Date;
            await _IGenericRepo.CreateNewContext();
            var result = await _IGenericRepo.Delete(course);
            return Json("Record deleted successfully.");
        }
    }
}