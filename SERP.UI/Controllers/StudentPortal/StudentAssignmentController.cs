using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.Student;
using SERP.Core.Model.StudentPortal;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;

namespace SERP.UI.Controllers.StudentPortal
{
    public class StudentAssignmentController : Controller
    {
        private readonly IGenericRepository<AssignmentModel, int> _assignmentRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<AssginmentUpload, int> _assignmentUploadRepo;
        public StudentAssignmentController(IGenericRepository<AssignmentModel, int> assignmentRepo,
          IHostingEnvironment hostingEnvironment, IGenericRepository<CourseMaster, int> _courseRepo,
           IGenericRepository<BatchMaster, int> _batchRepo, ISubjectMasterRepo _subjectRepo,
           IGenericRepository<BatchMaster, int> batchRepo, IGenericRepository<AssginmentUpload, int> assignmentUploadRepo)
        {
            _assignmentRepo = assignmentRepo;
            _hostingEnviroment = hostingEnvironment;
            _ICourseRepo = _courseRepo;
            _ISubjectRepo = _subjectRepo;
            _IBatchRepo = batchRepo;
            _assignmentUploadRepo = assignmentUploadRepo;
        }
        public async Task<IActionResult> Index()
        {
            var studentDetail = HttpContext.Session.GetObject<StudentAccountModel>("StudentInfo");
            var assignModel = await GetAssignmentList(studentDetail.CourseId, studentDetail.BatchId, studentDetail.StudentId);
            return PartialView("~/Views/StudentPortal/StudentAssignment/_StudentAssignmentPartial.cshtml",assignModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAssignment(IFormFile fileUpload,int hiddenId)
        {
            var studentDetail = HttpContext.Session.GetObject<StudentAccountModel>("StudentInfo");
            AssginmentUpload model = new AssginmentUpload();
            model.StudentId = studentDetail.StudentId;
            model.AssignmentId = hiddenId;
            model.AssignmentUploadDate = DateTime.Now.Date;


            if(fileUpload!=null)
            {
                var upload = Path.Combine(_hostingEnviroment.WebRootPath, "StudentDocument/");
                using (FileStream fs = new FileStream(Path.Combine(upload, fileUpload.FileName), FileMode.Create))
                {
                    await fileUpload.CopyToAsync(fs);
                }
                model.UploadAssignmentPath = "/StudentDocument/" + fileUpload.FileName;
            }
            
            var response = await _assignmentUploadRepo.CreateEntity(model);
            return Json("Assignment uploaded successfully.");

        }

        private async Task<List<AssignmentViewModel>> GetAssignmentList(int courseId, int batchId, int studentId)
        {
            var assigmentModels = (from AM in await _assignmentRepo.GetAll(x => x.IsActive)
                                   join BM in await _IBatchRepo.GetAll(x => x.IsActive == 1 && x.Id == batchId)
                                   on AM.BatchId equals BM.Id
                                   join CM in await _ICourseRepo.GetAll(x => x.IsActive == 1 && x.Id == courseId)
                                   on AM.CourseId equals CM.Id
                                   join SM in await _ISubjectRepo.GetAll(x => x.IsActive == 1)
                                   on AM.SubjectId equals SM.Id
                                   select new AssignmentViewModel
                                   {
                                       AssignmentId = AM.Id,
                                       CourseName = CM.Name,
                                       BatchName = BM.BatchName,
                                       AssignmentName = AM.AssignmentName,
                                       AssignDate = AM.AssignmentPublishDate,
                                       SubmissionDate = AM.SubmissionDate,
                                       PDFPath = AM.PDFPath,
                                       SubjectName = SM.SubjectName,
                                       IsUploaded=false
                                       
                                   }).ToList();

            var uploadAssignment = await _assignmentUploadRepo.GetList(x => x.StudentId == studentId);
            if (uploadAssignment.Count() > 0)
            {
                assigmentModels.ForEach(item =>
                {
                    uploadAssignment.ToList().ForEach(a =>
                    {
                        if(item.AssignmentId== a.AssignmentId)
                        {
                            item.AssigmentRemarks = a.AssigmentRemarks;
                            item.AssignmentGrade = a.AssignmentGrade;
                            item.AssignmentUploadDate = a.AssignmentUploadDate;
                            item.UploadAssignmentPath = a.UploadAssignmentPath;
                            item.IsUploaded = true;
                        }
                    });

                });
            }

            return assigmentModels;
        }
    }
}