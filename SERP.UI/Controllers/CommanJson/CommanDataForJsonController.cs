using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.CommanJson
{
    public class CommanDataForJsonController : Controller
    {
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly ITimeSheetRepo _timeSheetRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        public CommanDataForJsonController(ISubjectMasterRepo subjectRepo,
                                        IGenericRepository<BatchMaster, int> batchRepo,
                                        ITimeSheetRepo timeSheetRepo, IGenericRepository<EmployeeBasicInfoModel, int> basicInfoRepo)
        {
            _ISubjectRepo = subjectRepo;
            _IBatchRepo = batchRepo;
            _timeSheetRepo = timeSheetRepo;
            _basicInfoRepo = basicInfoRepo;
        }
        public async Task<IActionResult> SubjectJson(int courseId)
        {
            var result = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId);
            return Json(result);
        }
        public async Task<IActionResult> BatchJson(int courseId)
        {
            var result = await _IBatchRepo.GetList(x => x.IsActive == 1 && x.CourseId == courseId);
            return Json(result);
        }
        public async Task<IActionResult> TeacherJson(int subjectId)
        {
            var result = await _timeSheetRepo.GetSubjectTeacher(subjectId);
            return Json(result);
        }

        public async Task<IActionResult> TeacherJson()
        {
           var result= await _basicInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return Json(result);
        }
    }
}