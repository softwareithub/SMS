using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.HomeAssignment;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.AssignmentHomeModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Assignment
{
    public class StudyMaterialController : Controller
    {
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<StudyMaterial, int> _IStudyMaterialRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;

        public  StudyMaterialController(IGenericRepository<CourseMaster, int> courseRepo,
                                                 IGenericRepository<BatchMaster, int> batchRepo,
                                                 IGenericRepository<SubjectMaster, int> subjectRepo,
                                                 IGenericRepository<StudyMaterial, int> studyRepo,
                                                 IHostingEnvironment hostingEnviroment,
                                                 IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
                                                 )
        {
            _ICourseRepo = courseRepo;
            _IBatchRepo = batchRepo;
            _IStudyMaterialRepo = studyRepo;
            _ISubjectRepo = subjectRepo;
            _hostingEnviroment = hostingEnviroment;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/Documents/_StudyMaterialPartial.cshtml", await _IStudyMaterialRepo.GetSingle(x => x.IsActive == 1 && x.Id == id));
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 999999999)]
        [RequestSizeLimit(999999999)]

        public async Task<IActionResult> UploadDocument(StudyMaterial model,IFormFile MaterialPath)
        {
            List<IFormFile> formFiles = new List<IFormFile>();
            if (MaterialPath != null && MaterialPath.Length > 0)
            {
                formFiles.Add(MaterialPath);
                var imagePaths =await UploadImage.UploadImageOnFolder(formFiles, _hostingEnviroment);
                model.MaterialPath = imagePaths.First();
            }
            else {
                model.MaterialPath = string.Empty;
            }
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var response=await _IStudyMaterialRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else {
                var response = await _IStudyMaterialRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }

        }
        public async Task<IActionResult> GetDocuments()
        {
            try
            {
                var result = (from SM in await _IStudyMaterialRepo.GetList(x => x.IsActive == 1)
                              join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                              on SM.CourseId equals CM.Id
                              join BM in await _IBatchRepo.GetList(x => x.IsActive == 1)
                              on SM.BatchId equals BM.Id
                              join SB in await _ISubjectRepo.GetList(x => x.IsActive == 1)
                              on SM.SubjectId equals SB.Id
                              select new StudyMaterialModel
                              {
                                  Id = SM.Id,
                                  DocumentName = SM.MaterialName,
                                  Description = SM.MaterialDescription,
                                  CourseName = CM.Name,
                                  BatchName = BM.BatchName,
                                  SubjectName = SB.SubjectName,
                                  UploadType = SM.UploadType,
                                  Path = SM.MaterialPath,
                                  PublishDate = SM.PublishDate
                              }).ToList();

                return PartialView("~/Views/AssignmentWork/DocumentListPartial.cshtml", result);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));
            }
        }

        public async Task<IActionResult> DeleteDocument(int id)
        {
            var model = await _IStudyMaterialRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode(model, 1);
            await _IStudyMaterialRepo.CreateNewContext();
            var response = await _IStudyMaterialRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}