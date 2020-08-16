using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Assignment
{
    public class HomeWorkController : Controller
    {
        private readonly IGenericRepository<HomeWorkModel, int> _IHomeWorkRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public HomeWorkController(IGenericRepository<HomeWorkModel, int> homeworkRepo, 
            IGenericRepository<CourseMaster, int>  courseRepo, IGenericRepository<BatchMaster, int> batchRepo,
            IGenericRepository<SubjectMaster, int> subjectRepo,
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, 
            IGenericRepository<StudentMaster, int> studentRepo,
             IGenericRepository<StudentPromote, int>  studentPromte,
             IGenericRepository<ExceptionLogging, int> exceptionLogging
             )
        {
            _IHomeWorkRepo = homeworkRepo;
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _ISubjectRepo = subjectRepo;
            _IEmployeeRepo = employeeRepo;
            _IStudentMaster = studentRepo;
            _IStudentPromote = studentPromte;
            _exceptionLoggingRepo = exceptionLogging;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                var model = await _IHomeWorkRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/AssignmentWork/_HomeWorkPartialView.cshtml", model);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Index), nameof(HomeWorkController), ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(HomeWorkModel model)
        {
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var response = await _IHomeWorkRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now.Date;
                var response = await _IHomeWorkRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _IHomeWorkRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode(model,1);
            await _IHomeWorkRepo.CreateNewContext();
            var response = await _IHomeWorkRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }

        public async Task<IActionResult> GetList()
        {
            var result = (from HM in await _IHomeWorkRepo.GetList(x => x.IsActive == 1)
                          join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                          on HM.CourseId equals CM.Id
                          join BM in await _IBatchRepo.GetList(x => x.IsActive == 1)
                          on HM.BatchId equals BM.Id
                          join SM in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                          on HM.SubjectId equals SM.Id
                          join EM in await _IEmployeeRepo.GetList(x => x.IsActive == 1)
                          on HM.EmployeeId equals EM.Id
                          select new HomeWorkModelVm
                          {
                              Id= HM.Id,
                              CourseName= CM.Name,
                              BatchName= BM.BatchName,
                              SubjectName= SM.SubjectName,
                              EmployeeName= EM.Name,
                              HomeWork= HM.HomeWork,
                              PDFPath= HM.PDFPath,
                              PublishDate=Convert.ToDateTime(HM.HomeWorkDate),
                              SubmissionDate=Convert.ToDateTime(HM.HomeWorkSubmissionDate),
                          }).ToList();
            return PartialView("~/Views/AssignmentWork/_HomeWorkDetails.cshtml", result);            
        }

        
        public async Task<IActionResult> GetDetail(int id)
        {
            var response = await _IHomeWorkRepo.GetSingle(x => x.Id == id);
            return Json(response.HomeWork);
        }
        public async Task<IActionResult> GetBatchList(int id)
        {
            var data = await _IBatchRepo.GetList(z => z.CourseId == id);
            return Json(data);
        }
        public async Task<IActionResult> GetSubjectList(int id)
        {
            var data = await _ISubjectRepo.GetList(z => z.CourseId == id);
            return Json(data);
        }

        public async Task<IActionResult> GetTeachers(int courseId)
        {
            var models = await _IEmployeeRepo.GetList(x => x.IsActive == 1);
            return Json(models);
        }
    }
}