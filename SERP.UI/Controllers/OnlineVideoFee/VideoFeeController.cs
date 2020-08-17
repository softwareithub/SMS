using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.OnlineVideo;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.FeeDetails;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Utilities.ExceptionHelper;

namespace SERP.UI.Controllers.OnlineVideoFee
{
    public class VideoFeeController : Controller
    {
        private readonly IGenericRepository<OnlineVideoFeeDetail, int> _IOnlineVideoFeeRepo;
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<SubjectMaster, int> _ISubjectRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;
        
        public VideoFeeController(IGenericRepository<OnlineVideoFeeDetail, int> onlineVideoRepo,
            IGenericRepository<CourseMaster, int> courseRepo, IGenericRepository<SubjectMaster, int> _subjectRepo, IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IOnlineVideoFeeRepo = onlineVideoRepo;
            _ICourseRepo = courseRepo;
            _ISubjectRepo = _subjectRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.ClassList = await _ICourseRepo.GetList(x => x.IsActive == 1);
                var response = await _IOnlineVideoFeeRepo.GetSingle(x => x.Id == id);
                return PartialView("~/Views/OnlineVideoFee/_OnlineVideoFeeIndexPartial.cshtml", response);
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
        public async Task<IActionResult> CreateOnlineFee(OnlineVideoFeeDetail model)
        {
            if (model.Id > 0)
            {
                var updateModel = CommanDeleteHelper.CommanUpdateCode<OnlineVideoFeeDetail>(model, 1);
                var response = await _IOnlineVideoFeeRepo.Update(updateModel);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                var response = await _IOnlineVideoFeeRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> GetOnlineFeeDetails()
        {
            try
            {
                var response = (from VF in await _IOnlineVideoFeeRepo.GetList(x => x.IsActive == 1)
                                join CM in await _ICourseRepo.GetList(x => x.IsActive == 1)
                                on VF.CourseId equals CM.Id
                                select new OnlineFeeModel
                                {
                                    Id = VF.Id,
                                    CourseName = CM.Name,
                                    SubjectId = VF.SubjectId,
                                    Amount = VF.FeeAmount,
                                }).ToList();

                var subjectDetails = await _ISubjectRepo.GetList(x => x.IsActive == 1);

                response.ForEach(item =>
                {
                    if (item.SubjectId != 0)
                    {
                        item.SubjectName = subjectDetails.FirstOrDefault(x => x.Id == item.SubjectId).SubjectName;
                    }
                    else
                    {
                        item.SubjectName = item.CourseName;
                    }
                });

                return PartialView("~/Views/OnlineVideoFee/_OnlineFeeVideoDetailPartial.cshtml", response);
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

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _IOnlineVideoFeeRepo.GetSingle(x => x.Id == id);
            var deleteModel = CommanDeleteHelper.CommanDeleteCode<OnlineVideoFeeDetail>(model, 1);
            await _IOnlineVideoFeeRepo.CreateNewContext();
            var response = await _IOnlineVideoFeeRepo.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}