using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.SubjectTransaction
{
    public class SubjectMasterController : Controller
    {
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly ISubjectMasterRepo _ISubjectRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public SubjectMasterController(IGenericRepository<BatchMaster, int> batchRepo, IGenericRepository<CourseMaster, int> courseRepo,
            ISubjectMasterRepo subjectRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IBatchRepo = batchRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = subjectRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;

        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                var result = await _ISubjectRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/SubjectMaster/_AddSubjectMasterIndexPartial.cshtml", result);
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

        public async Task<IActionResult> AddSubjectPartial(int classId, int batchId)
        {
            try
            {
                TempData["CourseId"] = classId;
                TempData["BatchId"] = batchId;
                return await Task.Run(() => PartialView("~/Views/SubjectMaster/SubjectMasterPartial.cshtml"));
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
        public async Task<IActionResult> PostAddSubject(SubjectMaster model)
        {
            try
            {
                //Delete Records
                if (model.Id > 0)
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _ISubjectRepo.Update(model);
                    return Json(ResponseData.Instance.GenericResponse(result));
                }
                else
                {
                    var result = await _ISubjectRepo.CreateEntity(model);
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

        public async Task<IActionResult> GetSubjectDatList()
        {
            try
            {
                var subjectModel = await _ISubjectRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                var courseModel = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

                var result = (from SM in subjectModel
                              join CM in courseModel
                              on SM.CourseId equals CM.Id
                              where CM.IsDeleted == 0 && CM.IsActive == 1
                              && SM.IsActive == 1 && SM.IsDeleted == 0
                              select new SubjectVm
                              {
                                  SubjectId = SM.Id,
                                  CourseName = CM.Name,
                                  SubjectCode = SM.SubjectCode,
                                  SubjectName = SM.SubjectName,
                                  SubjectDescription = SM.SubjectDescription,
                                  IsElective = SM.IsElective
                              }).ToList();

                return PartialView("~/Views/SubjectMaster/SubjectMasterPartial.cshtml", result);
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

        #region PrivateFields
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                var subjectModel = await _ISubjectRepo.GetSingle(x => x.Id == id);
                subjectModel.IsActive = 0;
                subjectModel.IsDeleted = 1;
                subjectModel.UpdatedBy = 1;
                subjectModel.UpdatedDate = DateTime.Now.Date;
                await _ISubjectRepo.CreateNewContext();
                var result = await _ISubjectRepo.Delete(subjectModel);
                return Json(ResponseData.Instance.GenericResponse(result));
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
        #endregion

    }
}