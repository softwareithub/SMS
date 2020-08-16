using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.StudentTransaction
{
    public class StudentIdController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _IStudentMasterRepo;
        private readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;

        public StudentIdController(IGenericRepository<StudentMaster, int> studentRepo, IGenericRepository<StudentPromote, int>studentPromote, IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<BatchMaster, int> batchRepo)
        {
            _IStudentMasterRepo = studentRepo;
            _IStudentPromote = studentPromote;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/StudentIDCard/_StudentIDCardPartial.cshtml");
        }

        public async Task<IActionResult>GetStudentIDCard(int  batchId)
        {
            List<StudentPartialInfoViewModel> models = new List<StudentPartialInfoViewModel>();
            models = (from SP in await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.BatchId == batchId)
                      join SM in await _IStudentMasterRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1)
                      on SP.StudentId equals SM.Id
                      join CM in await _ICourseRepo.GetList(x=>x.IsActive==1 && x.IsDeleted==0)
                      on SP.CourseId equals  CM.Id
                      join BM in await _IBatchRepo.GetList(x=>x.IsActive==1 && x.IsDeleted==0)
                      on SP.BatchId equals BM.Id
                      select new StudentPartialInfoViewModel
                      {
                          Id = SM.Id,
                          StudentName= SM.Name,
                          FatherName=SM.FatherName,
                          RollCode= SM.RollCode,
                          CourseName= CM.Name,
                          BatchName=BM.BatchName,
                          FatherPhone= SM.FatherPhone,
                          StudentPhoto=SM.StudentPhoto,
                          C_Address=SM.C_Address
                      }).ToList();

            return PartialView("~/Views/StudentIDCard/_StudentIDCardDetails.cshtml", models);
        }
    }
}