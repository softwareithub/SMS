using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.API.Model;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudyMaterialController : ControllerBase
    {
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;
        public readonly IGenericRepository<StudyMaterial, int> _IStudyMaterialRepo;
        public readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<HomeWorkModel, int> _IHomeWorkRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        public StudyMaterialController(IGenericRepository<StudentPromote, int>  studentPromoteRepo,
            IGenericRepository<StudyMaterial, int> studyRepo,
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<HomeWorkModel, int> homeWorkRepo,
            IGenericRepository<CourseMaster, int> courseRepo,
            IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo
            )
        {
            _IStudentPromoteRepo = studentPromoteRepo;
            _IStudyMaterialRepo = studyRepo;
            _ISubjectRepo = subjectRepo;
            _IHomeWorkRepo = homeWorkRepo;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _IEmployeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjetc(int studentId)
        {
            var studentDetail = await _IStudentPromoteRepo.GetSingle(x => x.StudentId == studentId && x.IsActive==1);
            return Ok(await _ISubjectRepo.GetList(x => x.CourseId == studentDetail.CourseId && x.IsActive==1));
        }

        [HttpGet]
        public async Task<IActionResult>GetStudyMaterial(int subjectId)
        {
            return Ok(await _IStudyMaterialRepo.GetList(z => z.IsActive == 1 && z.SubjectId == subjectId));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocument(int id)
        {
            return Ok(await _IStudyMaterialRepo.GetSingle(x => x.Id == id));
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetYouTubePlayList()
        {
            return Ok(new PlayListModel().GetPlayList());
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetHomeWorkDetails(int studentId)
        {
            var studentDetail = await _IStudentPromoteRepo.GetSingle(x => x.StudentId == studentId && x.IsActive == 1);

            var result = (from HM in await _IHomeWorkRepo.GetList(x => x.IsActive == 1)
                          join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                          on HM.CourseId equals CM.Id
                          join BM in await _IBatchRepo.GetList(x => x.IsActive == 1)
                          on HM.BatchId equals BM.Id
                          join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                          on HM.SubjectId equals SM.Id
                          join EM in await _IEmployeeRepo.GetList(x => x.IsActive == 1)
                          on HM.EmployeeId equals EM.Id
                          where HM.CourseId== studentDetail.CourseId && HM.BatchId== studentDetail.BatchId
                           
                          select new HomeWorkModelVm
                          {
                              Id = HM.Id,
                              CourseName = CM.Name,
                              BatchName = BM.BatchName,
                              SubjectName = SM.SubjectName,
                              EmployeeName = EM.Name,
                              HomeWork = HM.HomeWork,
                              PDFPath = HM.PDFPath,
                              PublishDate = Convert.ToDateTime(HM.HomeWorkDate),
                              SubmissionDate = Convert.ToDateTime(HM.HomeWorkSubmissionDate),
                          }).OrderByDescending(x=>x.Id).ToList();

            return Ok(result);
        }

    }
}