using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.SERPExceptionLogging;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ExceptionHelper;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.UserManagement
{
    public class UserLoginController : Controller
    {
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        private readonly IGenericRepository<Authenticate, int> _IAuthenticate;
        private readonly IGenericRepository<StudentMaster, int> _IStudentRepo;
        private readonly IGenericRepository<RoleMaster, int> _roleMasterRepo;
        private readonly IGenericRepository<ExceptionLogging, int> _exceptionLoggingRepo;



        public UserLoginController(IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<Authenticate, int> authenticateRepo, IGenericRepository<StudentMaster, int> studentRepo,
            IGenericRepository<RoleMaster, int> roleMasterRepo,
                    IGenericRepository<ExceptionLogging, int> exceptionLoggingRepo)
        {
            _IEmployeeRepo = employeeRepo;
            _IAuthenticate = authenticateRepo;
            _IStudentRepo = studentRepo;
            _roleMasterRepo = roleMasterRepo;
            _exceptionLoggingRepo = exceptionLoggingRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                ViewBag.Employee = await _IEmployeeRepo.GetList(x => x.IsActive == 1);
                ViewBag.StudentList = await _IStudentRepo.GetList(x => x.IsActive == 1);
                ViewBag.Rolelist = await _roleMasterRepo.GetList(x => x.IsActive == 1);
                return PartialView("~/Views/UserManagement/_UserLoginCreatePartial.cshtml", await _IAuthenticate.GetSingle(x => x.Id == id));
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
        public async Task<IActionResult> CreateLogin(Authenticate model)
        {
            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                model.LastLoginDateTime = DateTime.Now;
                var response = await _IAuthenticate.Update(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
            else
            {
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now.Date;
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                model.LastLoginDateTime = DateTime.Now;
                var response = await _IAuthenticate.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(response));
            }
        }

        public async Task<IActionResult> GetList()
        {
            try
            {
                List<UserLoginModel> models = new List<UserLoginModel>();
                var authenticateModels = await _IAuthenticate.GetList(x => x.IsActive == 1);
                var studentModels = await _IStudentRepo.GetList(x => x.IsActive == 1);
                var employeeList = await _IEmployeeRepo.GetList(x => x.IsActive == 1);

                authenticateModels.ToList().ForEach(item =>
                {
                    if (item.StudentId != 0)
                    {
                        models.Add(new UserLoginModel
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            Password = item.Password,
                            IsExpired = item.IsExpired,
                            IsActive = item.IsActive,
                            IsLocked = item.IsLocked,
                            StudentName = studentModels.ToList().Find(x => x.Id == item.StudentId).Name
                        }); ;


                    }
                    else
                    {
                        models.Add(new UserLoginModel
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            Password = item.Password,
                            IsExpired = item.IsExpired,
                            IsActive = item.IsActive,
                            IsLocked = item.IsLocked,
                            EmployeeCode = employeeList.ToList().Find(x => x.Id == item.EmployeeId).EmpCode,
                            EmployeeName = employeeList.ToList().Find(x => x.Id == item.EmployeeId).Name
                        });
                    }

                });
                return PartialView("~/Views/UserManagement/_UserManagementList.cshtml", models);
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
            var model = await _IAuthenticate.GetSingle(x => x.Id == id);
            var deleteModel = Utilities.CommanHelper.CommanDeleteHelper.CommanDeleteCode(model, 1);
            await _IAuthenticate.CreateNewContext();
            var response = await _IAuthenticate.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}