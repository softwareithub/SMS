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
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Helper;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Assignment
{
    public class AssignmentMasterController : Controller
    {
        private readonly IGenericRepository<AssignmentModel, int> _assignmentRepo;
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        public AssignmentMasterController(IGenericRepository<AssignmentModel, int> assignmentRepo,
            IHostingEnvironment hostingEnvironment, IGenericRepository<CourseMaster, int> _courseRepo,
             IGenericRepository<BatchMaster, int> _batchRepo, ISubjectMasterRepo _subjectRepo, IGenericRepository<BatchMaster, int> batchRepo, IGenericRepository<ExceptionLogging, int>  exceptionLogging)
        {
            _assignmentRepo = assignmentRepo;
            _hostingEnviroment = hostingEnvironment;
             _ICourseRepo= _courseRepo;
            _ISubjectRepo = _subjectRepo;
            _IBatchRepo = batchRepo;
            _exceptionLoggingRepo = exceptionLogging;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                await PopulateViewBag();
                var model = await _assignmentRepo.GetSingle(x => x.Id == id);
                HttpContext.Session.SetString("pdfPath", string.IsNullOrEmpty(model?.PDFPath) ? string.Empty : model.PDFPath);
                return View("~/Views/AssignmentWork/_AssignCreate.cshtml", model);
            }
            catch(Exception ex)
            {
                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(nameof(Index), nameof(AssignmentMasterController), ex.Message,LoggingType.httpGet.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return await Task.Run(() => PartialView("~/Views/Shared/Error.cshtml"));

            }

        }

        public async Task<IActionResult> CreateAssignment(AssignmentModel model, IFormFile PDFPath)
        {
            try
            {
                if (model.Id == 0)
                {
                    List<IFormFile> formFiles = new List<IFormFile>();
                    formFiles.Add(PDFPath);
                    var pdfPath = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnviroment);
                    model.PDFPath = pdfPath.First();
                    var result = await _assignmentRepo.CreateEntity(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    model.PDFPath = string.IsNullOrEmpty(model.PDFPath) ? HttpContext.Session.GetString("pdfPath") : model.PDFPath;
                    var result = await _assignmentRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }

        public async Task<IActionResult> AssignmentList()
        {
            try
            {
                return PartialView("~/Views/AssignmentWork/_AssignmentList.cshtml", await GetAssignmentList());
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

        public async Task<IActionResult> GetAssignDetails(int id)
        {
            try
            {
                var result = await GetAssignmentList();
                return Json(result.Find(x => x.AssignmentId == id).Assignment);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var exceptionHelper = new LoggingHelper().GetExceptionLoggingObj(actionName, controllerName, ex.Message, LoggingType.httpDelete.ToString(), 0);
                var exceptionResponse = await _exceptionLoggingRepo.CreateEntity(exceptionHelper);
                return Json(ResponseData.Instance.GenericResponse(ResponseStatus.ServerError));
            }
        }
        private async Task PopulateViewBag()
        {
            ViewBag.CourseList = await _ICourseRepo.GetAll(x => x.IsActive == 1);
        }

        private async Task<List<SERP.Core.Model.AssignmentHomeModel.AssignmentModel>> GetAssignmentList()
        {

                var result = (from AM in await _assignmentRepo.GetAll(x => x.IsActive)
                              join BM in await _IBatchRepo.GetAll(x => x.IsActive == 1)
                              on AM.BatchId equals BM.Id
                              join CM in await _ICourseRepo.GetAll(x => x.IsActive == 1)
                              on AM.CourseId equals CM.Id
                              join SM in await _ISubjectRepo.GetAll(x => x.IsActive == 1)
                              on AM.SubjectId equals SM.Id
                              select new SERP.Core.Model.AssignmentHomeModel.AssignmentModel
                              {
                                  AssignmentId = AM.Id,
                                  CourseName = CM.Name,
                                  BatchName = BM.BatchName,
                                  AssignmentName = AM.AssignmentName,
                                  PublishDate = AM.AssignmentPublishDate,
                                  SubmissionDate = AM.SubmissionDate,
                                  PDFPath = AM.PDFPath,
                                  Assignment = AM.Assignment,
                                  SubjectName = SM.SubjectName
                              }).ToList();

                return result;
          
        }

    }
}