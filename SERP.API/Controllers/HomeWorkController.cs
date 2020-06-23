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
    public class HomeWorkController : ControllerBase
    {
        private readonly IGenericRepository<HomeWorkModel, int> _IHomeWorkRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromoteRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<StudentHomeWork, int> _IStudentHomeWorkRepo;


        public HomeWorkController(IGenericRepository<HomeWorkModel, int> homeworkRepo,
            IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<StudentMaster, int> studentRepo,
             IGenericRepository<StudentPromote, int> studentPromte,
             IGenericRepository<StudentHomeWork, int> studentHomeWorkRepo)
        {
            _IHomeWorkRepo = homeworkRepo;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _ISubjectRepo = subjectRepo;
            _IEmployeeRepo = employeeRepo;
            _IStudentMaster = studentRepo;
            _IStudentPromote = studentPromte;
            _IStudentHomeWorkRepo = studentHomeWorkRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeWorkDetail(int studentId)
        {
            var studentPromote = await _IStudentPromote.GetSingle(x => x.StudentId == studentId && x.IsActive == 1);
            List<HomeWorkModelVm> result = await HomeWorkDetails(studentPromote);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeWorkReport(int studentId)
        {
            List<StudentHomeWorkModel> models = await HomeWorkReport(studentId);
            return Ok(models);
        }

       

        #region PrivateFields
        private async Task<List<HomeWorkModelVm>> HomeWorkDetails(StudentPromote studentPromote)
        {
            return (from HM in await _IHomeWorkRepo.GetList(x => x.IsActive == 1)
                    join CM in await _ICourseRepo.GetList(x => x.IsActive == 1 && x.Id == studentPromote.CourseId)
                    on HM.CourseId equals CM.Id
                    join BM in await _IBatchRepo.GetList(x => x.IsActive == 1 && x.Id == studentPromote.BatchId)
                    on HM.BatchId equals BM.Id
                    join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                    on HM.SubjectId equals SM.Id
                    join EM in await _IEmployeeRepo.GetList(x => x.IsActive == 1)
                    on HM.EmployeeId equals EM.Id
                    select new HomeWorkModelVm
                    {
                        Id = HM.Id,
                        CourseName = CM.Name,
                        BatchName = BM.BatchName,
                        SubjectName = SM.SubjectName,
                        EmployeeName = EM.Name,
                        HomeWork = HM.HomeWork,
                        PDFPath = HM.PDFPath,
                        PublishDate = HM.HomeWorkDate,
                        SubmissionDate = HM.HomeWorkSubmissionDate,
                    }).ToList();
        }

        private async Task<List<StudentHomeWorkModel>> HomeWorkReport(int studentId)
        {
            return (from HM in await _IHomeWorkRepo.GetList(x => x.IsActive == 1)
                    join HMS in await _IStudentHomeWorkRepo.GetList(x => x.IsActive == 1 && x.StudentId == studentId)
                    on HM.Id equals HMS.HomeWorkId
                    join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                    on HM.SubjectId equals SM.Id
                    select new StudentHomeWorkModel
                    {
                        HomeWorkName = HM.Name,
                        HomeWorkDate = HM.HomeWorkDate,
                        SubmissionDate = HM.HomeWorkSubmissionDate,
                        ActualSubmissionDate = HMS.SubmissionDate,
                        Grade = HMS.Grade,
                        Notes = HMS.Reason
                    }).ToList();
        }
        #endregion
    }
}