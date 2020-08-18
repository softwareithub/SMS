using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.CommanHelper;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.StudentTransaction
{
    public class StudentMasterController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _IStudentMaster;
        private readonly IGenericRepository<BatchMaster, int> _IBatchMaster;
        private readonly IGenericRepository<CourseMaster, int> _ICourseMaster;
        private readonly IGenericRepository<ReligionMaster, int> _IReligionMaster;
        private readonly IGenericRepository<CasteMaster, int> _ICategoryMaster;
        private readonly IGenericRepository<AcademicMaster, int> _IAcademicRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGenericRepository<FeeDiscountModel, int> _IFeeDiscountRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;


        public StudentMasterController(IGenericRepository<StudentMaster, int> studentRepo,
           IGenericRepository<BatchMaster, int> batchRepo,
           IGenericRepository<CourseMaster, int> courseRepo,
           IGenericRepository<ReligionMaster, int> religionRepo,
           IGenericRepository<CasteMaster, int> categoryRepo,
           IGenericRepository<AcademicMaster, int> academicRepo,
            IGenericRepository<StudentPromote, int> studentPromote,
            IHostingEnvironment hostingEnvironment,
            IGenericRepository<FeeDiscountModel, int> feeDiscountRepo,
            IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo
           )
        {
            _IStudentMaster = studentRepo;
            _IBatchMaster = batchRepo;
            _ICourseMaster = courseRepo;
            _IReligionMaster = religionRepo;
            _ICategoryMaster = categoryRepo;
            _IAcademicRepo = academicRepo;
            _IStudentPromote = studentPromote;
            _hostingEnvironment = hostingEnvironment;
            _IFeeDiscountRepo = feeDiscountRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;

        }
        public async Task<IActionResult> CreateStudent(int id = 0)
        {
            try
            {
                await PopulateViewBag();
                if (id == 0)
                    return await Task.Run(() => PartialView("~/Views/StudentMaster/_StudentMasterPartial.cshtml"));

                var studentPromote = await _IStudentPromote.GetSingle(x => x.StudentId == id);
                await _IStudentPromote.CreateNewContext();
                var studentModel = await _IStudentMaster.GetSingle(x => x.Id == studentPromote.StudentId);

                return PartialView("~/Views/StudentMaster/_StudentMasterPartial.cshtml", studentModel);
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

        public async Task<IActionResult> GetBatchList(int courseId)
        {
            try
            {
                var batchList = await _IBatchMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.CourseId == courseId);
                return Json(batchList);
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

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentMaster model, IFormFile StudentPhoto, IFormFile ParentsPhoto)
        {
            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                formFiles.Add(StudentPhoto);
                formFiles.Add(ParentsPhoto);
                var imagePaths = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnvironment);
                if (imagePaths.Count() == 0)
                {
                    model.ParentsPhoto = string.Empty;
                    model.StudentPhoto = string.Empty;
                }
                else
                {
                    if (imagePaths.Count() == 1)
                    {
                        model.StudentPhoto = string.IsNullOrEmpty(model.StudentPhoto) ? imagePaths[0] : model?.StudentPhoto;
                        model.ParentsPhoto = string.Empty;
                    }
                    else if (imagePaths.Count() == 2)
                    {
                        model.StudentPhoto = string.IsNullOrEmpty(model.StudentPhoto) ? imagePaths[0] : model?.StudentPhoto;
                        model.ParentsPhoto = string.IsNullOrEmpty(model.ParentsPhoto) ? imagePaths[1] : model?.ParentsPhoto;
                    }


                }


                if (model.Id == 0)
                {
                    model.CreatedBy = 1;
                    return await CreateStudentEntity(model);
                }
                else
                {
                    model.UpdatedBy = 1;
                    model.UpdatedDate = DateTime.Now.Date;
                    var result = await _IStudentMaster.Update(model);
                    if (result != ResponseStatus.UpdatedSuccessFully)
                    {
                        return Json(ResponseData.Instance.GenericResponse(result));
                    }
                    else
                    {
                        var studentPromote = await _IStudentPromote.GetSingle(x => x.IsActive == 1 && x.IsDeleted == 0 && x.StudentId == model.Id);
                        studentPromote.CourseId = model.CourseId;
                        studentPromote.BatchId = model.BatchId;
                        studentPromote.UpdatedBy = 1;
                        studentPromote.UpdatedDate = DateTime.Now.Date;
                        await _IStudentPromote.CreateNewContext();
                        var studentPromoteResult = await _IStudentPromote.Update(studentPromote);
                        return Json(ResponseData.Instance.GenericResponse(studentPromoteResult));
                    }
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

        public async Task<IActionResult> GetStudentList()
        {
            try
            {
                List<StudentPartialInfoViewModel> studentViewModel = await GetStudentVm();

                return PartialView("~/Views/StudentMaster/_StudentlistPartial.cshtml", studentViewModel);
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

        public async Task<IActionResult> GetStudentCompleteList(int id)
        {
            try
            {
                var result = await GetStudentVm();
                var studentModel = result.FirstOrDefault(x => x.Id == id);
                return PartialView("~/Views/StudentMaster/_StudentProfilePartial.cshtml", studentModel);
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

        #region privateFields
        private async Task PopulateViewBag()
        {
            ViewBag.CourseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.BatchList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            ViewBag.ReligionList = await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.CategoryList = await _IFeeDiscountRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            ViewBag.AcademicList = await _IAcademicRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
        }

        public async Task<List<StudentPartialInfoViewModel>> GetStudentVm()
        {
            var studentPromotes = await _IStudentPromote.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentPromote.CreateNewContext();
            var studentList = await _IStudentMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            await _IStudentMaster.CreateNewContext();
            var courseList = await _ICourseMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentMaster.CreateNewContext();
            var sectionList = await _IBatchMaster.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            await _IStudentMaster.CreateNewContext();
            var religionList = await _IReligionMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IStudentMaster.CreateNewContext();
            var categoryList = await _IFeeDiscountRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);


            var studentViewModel = (from sp in studentPromotes
                                    from sl in studentList
                                    where (sp.StudentId == sl.Id)
                                    join Cl in courseList
                                    on sp.CourseId equals Cl.Id
                                    join bl in sectionList
                                    on sp.BatchId equals bl.Id
                                  
                                    select new StudentPartialInfoViewModel
                                    {
                                        Id = sl.Id,
                                        RollCode = sl.RollCode,
                                        Registration = sl.RegistrationNumber,
                                        CourseName = Cl.Name,
                                        BatchName = bl.BatchName,
                                        StudentName = sl.Name,
                                        DateOfBirth = sl.DateOfBirth,
                                        JoiningDate = sl.JoiningDate,
                                        Category = "",
                                        FatherEmail = sl.FatherEmail,
                                        StudentEmail = sl.StudentEmail,
                                        FatherPhone = sl.FatherPhone,
                                        StudentPhoto = sl.StudentPhoto,
                                        StudentPhone = sl.StudentPhone,
                                        MotherPhone = sl.MotherPhone,
                                        MotherName = sl.MotherName,
                                        P_Address = sl.P_Address,
                                        C_Address = sl.C_Address,
                                        Religion = "",
                                        ParentPhoto = sl.ParentsPhoto
                                    }).ToList();
            return studentViewModel;
        }

        public async Task<IActionResult> ValidateStudentRegistratioNumber(string registration)
        {
            try
            {
                if (!string.IsNullOrEmpty(registration))
                {
                    var studentModel = await _IStudentMaster.GetList(x => x.IsActive == 1 && x.RegistrationNumber.Trim().ToLower() == registration.Trim().ToLower());
                    if (studentModel.Count() == 0)
                    {
                        return Json("0");
                    }
                    else
                    {
                        return Json("1");
                    }
                }
                return Json("0");
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

        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var model = await _IStudentMaster.GetSingle(x => x.Id == id);
                var deleteModel = CommanDeleteHelper.CommanDeleteCode<StudentMaster>(model, 1);
                await _IStudentMaster.CreateNewContext();
                var response = await _IStudentMaster.Update(deleteModel);
                return Json(ResponseData.Instance.GenericResponse(response));
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

        private async Task<IActionResult> CreateStudentEntity(StudentMaster model)
        {
            try 
            {
                var result = await _IStudentMaster.CreateEntity(model);

                if (result != ResponseStatus.AddedSuccessfully)
                    return Json(ResponseData.Instance.GenericResponse(result));

                var students = await _IStudentMaster.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

                var studentId = students.Max(x => x.Id);

                StudentPromote studentPromote = new StudentPromote()
                {
                    CourseId = model.CourseId,
                    BatchId = model.BatchId,
                    PromotionDate = DateTime.Now.Date,
                    StudentId = studentId,
                    CreatedDate = DateTime.Now.Date
                };
                var studentResult = await _IStudentPromote.CreateEntity(studentPromote);

                return Json(ResponseData.Instance.GenericResponse(studentResult));
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