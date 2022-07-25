using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.ExamModel;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamSheetController : ControllerBase
    {

        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        private readonly IGenericRepository<Exam, int> _examRepo;
        private readonly IGenericRepository<ExamSheet, int> _IExamSheetRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;

        private readonly IGenericRepository<StudentMarkAllocation, int> _IStudentMarkRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;

        private readonly IGenericRepository<GradeMaster, int> _IGradeRepo;
        private readonly IGenericRepository<InstituteMaster, int> _IInstituteRepo;



        public ExamSheetController(IGenericRepository<BatchMaster, int> batchRepo,
           IGenericRepository<CourseMaster, int> courseRepo,
           ISubjectMasterRepo subjectRepo,
           IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
           IGenericRepository<Exam, int> examRepo, IGenericRepository<ExamSheet, int> examSheetRepo,
           IGenericRepository<StudentPromote, int> studentPromote,
           IGenericRepository<StudentMarkAllocation, int> studentMarkRepo,
           IGenericRepository<StudentMaster, int> studentMasterRepo,
           IGenericRepository<GradeMaster, int> gradeRepo,
           IGenericRepository<InstituteMaster, int> instituteRepo
          )
        {
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _ISubjectRepo = subjectRepo;
            _basicInfoRepo = employeeRepo;
            _examRepo = examRepo;
            _IExamSheetRepo = examSheetRepo;
            _IStudentPromote = studentPromote;
            _IStudentMarkRepo = studentMarkRepo;
            _IStudentMaster = studentMasterRepo;
            _IGradeRepo = gradeRepo;
            _IInstituteRepo = instituteRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetExamDetails()
        {
            return Ok(await _examRepo.GetList(x => x.IsActive == 1));
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeSheet(int examId, int studentId)
        {
            var response = await _IStudentPromote.GetSingle(x => x.StudentId == studentId && x.IsActive == 1 && x.IsDeleted == 0);
            var examResponse = await GetExamList(examId, response.CourseId);
            return Ok(examResponse);

        }

        private async Task<List<ExamSheetVm>> GetExamList(int examId, int courseId)
        {
            var result = (from ES in await _IExamSheetRepo.GetList(x => x.IsActive == 1 && x.ExamId == examId && x.CourseId == courseId)
                          join EM in await _examRepo.GetList(x => x.IsActive == 1)
                          on ES.ExamId equals EM.Id
                          join CM in await _ICourseMaster.GetList(x => x.IsActive == 1)
                          on ES.CourseId equals CM.Id
                          join BM in await _IBatchMaster.GetList(x => x.IsActive == 1)
                          on ES.BatchId equals BM.Id
                          join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                          on ES.SubjectId equals SM.Id
                          join EMP in await _basicInfoRepo.GetList(x => x.IsActive == 1)
                          on ES.InveligitatorId equals EMP.Id
                          select new ExamSheetVm
                          {
                              ExamId = ES.Id,
                              ExamName = EM.ExamName,
                              CourseName = CM.Name,
                              BatchName = BM.BatchName,
                              SubjectName = SM.SubjectName,
                              SubjectCode = SM.SubjectCode,
                              ExamDate = ES.ExamDate,
                              StartTime = ES.StartTime,
                              EndTime = ES.EndTime,
                              TeacherName = EMP.Name,
                              MaxMark = ES.MaxMark,
                              PassMark = ES.PassMark,
                              ExamDateStr = ES.ExamDate.ToLongDateString(),
                              ExamFromTime = ES.StartTime.ToString(),
                              ExamToTime = ES.EndTime.ToString()
                          }).ToList();

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> StudentMarkSheet(int studentId, int examId)
        {
            var examsheetList = await _IExamSheetRepo.GetList(x => x.IsActive == 1 && x.ExamId == examId);

            StudentMarkSheetVm model = new StudentMarkSheetVm();
            var gradeList = await _IGradeRepo.GetList(x => x.IsActive == 1);
            model.InstituteModel = await _IInstituteRepo.GetSingle(x => x.IsActive == 1);

            model.StudentInfoModel = (from SP in await _IStudentPromote.GetList(x => x.IsActive == 1)
                                      join SM in await _IStudentMaster.GetList(x => x.IsActive == 1)
                                      on SP.StudentId equals SM.Id
                                      join CM in await _ICourseMaster.GetList(x => x.IsActive == 1)
                                      on SP.CourseId equals CM.Id
                                      join BM in await _IBatchMaster.GetList(x => x.IsActive == 1)
                                      on SP.BatchId equals BM.Id

                                      select new StudentPartialInfoViewModel
                                      {
                                          Id = SP.StudentId,
                                          RollCode = SM.RollCode,
                                          Registration = SM.RegistrationNumber,
                                          CourseName = CM.Name,
                                          BatchName = BM.BatchName,
                                          StudentName = SM.Name,
                                          FatherName = SM.FatherName,
                                          MotherName = SM.MotherName,
                                          StudentPhoto = SM.StudentPhoto,
                                          DateOfBirth = SM.DateOfBirth,
                                          Gender = SM.Gender,
                                          JoiningDate = SM.JoiningDate
                                      }).ToList().Where(x => x.Id == studentId).FirstOrDefault();

            model.ExamModel = await _examRepo.GetSingle(x => x.Id == examId);

            model.MarkAllocationVms = (from EM in await _IStudentMarkRepo.GetList(x => x.IsActive == 1
                                                                  && x.ExamId == examId && x.StudentId == studentId)
                                       join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                                       on EM.SubjectId equals SM.Id
                                       //join ES in await _IExamSheetRepo.GetList(x => x.IsActive == 1 && x.ExamId == examId)
                                       //on EM.ExamId equals ES.ExamId
                                       
                                       
                                       select new StudentMarkAllocationVm
                                       {
                                           SubjectName = SM.SubjectName,
                                           PassMark = examsheetList.First(x =>x.SubjectId==SM.Id).PassMark,
                                           MaxMarks = examsheetList.Where(x => x.SubjectId == SM.Id).First().MaxMark,
                                           AssignedMarks = EM.AssignedMark,
                                           LabMarks = EM.LabMarks,
                                           Percentage = ((EM.AssignedMark + EM.LabMarks) / examsheetList.Where(x => x.SubjectId == SM.Id).First().MaxMark) * 100,
                                           Grade = GradeCalcullation(Convert.ToInt32(EM.AssignedMark + EM.LabMarks), gradeList.ToList())

                                       }).ToList();
            return Ok(model.MarkAllocationVms.GroupBy(x => x.SubjectName).Select(x => new StudentMarkAllocationVm
            {
                SubjectName = x.First().SubjectName,
                PassMark = x.First().PassMark,
                MaxMarks = x.First().MaxMarks,
                AssignedMarks = x.First().AssignedMarks,
                LabMarks = x.First().LabMarks,
                Percentage = x.First().Percentage,
                Grade = x.First().Grade

            }).ToList());
        }


        private string GradeCalcullation(int marks, List<GradeMaster> gradeMasters)
        {
            string grade = string.Empty;

            gradeMasters.ToList().ForEach(item =>
            {
                if (Convert.ToInt32(item.FromMarks) >= marks && marks <= Convert.ToInt32(item.ToMarks))
                {
                    grade = item.GradeName;
                }
            });
            return grade;
        }
    }
}
