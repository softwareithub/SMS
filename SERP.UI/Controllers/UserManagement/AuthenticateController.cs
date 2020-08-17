using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;
using SERP.Utilities.EmailHelper;

namespace SERP.UI.Controllers.UserManagement
{
    public class AuthenticateController : Controller
    {
        private readonly IGenericRepository<Authenticate, int> _IAuthenticateRepo;
        private readonly IGenericRepository<ModuleMaster, int> _IModuleRepo;
        private readonly IGenericRepository<SubModuleMaster, int> _ISubModuleRepo;
        private readonly IGenericRepository<UserAccessRight, int> _IUserAccessRight;
        private readonly IGenericRepository<InstituteMaster, int> _IInstituteRepo;
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        private readonly IGenericRepository<StudentPromote, int> _studentPromoteRepo;
        private readonly IGenericRepository<CourseMaster, int> _courseRepo;
        private readonly IGenericRepository<BatchMaster, int> _batchRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<AcademicMaster, int> _sessionRepo;
        public AuthenticateController(IGenericRepository<Authenticate, int> authenticateRepo,
            IGenericRepository<ModuleMaster, int> moduleRepo,
            IGenericRepository<SubModuleMaster, int> subModuleRepo,
            IGenericRepository<UserAccessRight, int> userAcccessRight,
            IGenericRepository<InstituteMaster, int> instituteRepo,
            IGenericRepository<StudentMaster, int> studentMasterRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo,
            IGenericRepository<CourseMaster, int> courseRepo,
            IGenericRepository<BatchMaster, int> batchRepo, 
            IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<AcademicMaster, int> sessionRepo
            )
        {
            _IAuthenticateRepo = authenticateRepo;
            _IModuleRepo = moduleRepo;
            _ISubModuleRepo = subModuleRepo;
            _IUserAccessRight = userAcccessRight;
            _IInstituteRepo = instituteRepo;
            _studentMasterRepo = studentMasterRepo;
            _studentPromoteRepo = studentPromoteRepo;
            _courseRepo = courseRepo;
            _batchRepo = batchRepo;
            _employeeRepo = employeeRepo;
            _sessionRepo = sessionRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string message, string returnUrl)
        {
            ViewBag.message = message;
            var instituteModel = await _IInstituteRepo.GetSingle(x => x.IsActive == 1);
            ViewBag.Logo = instituteModel.InstituteLogo;
            ViewBag.Name = instituteModel.Name;
            ViewBag.Rythum = instituteModel.Rythum;
            HttpContext.Session.SetString("InstituteName", instituteModel.Name);
            HttpContext.Session.SetString("InstituteLogo", instituteModel.InstituteLogo);
            ViewBag.SessionList = await _sessionRepo.GetAll(x => x.IsActive == 1);
            return await Task.Run(() => View("~/Views/UserManagement/_LoginPartial.cshtml"));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Authenticate model)
        {
            var response = await _IAuthenticateRepo.GetSingle(x => x.UserName == model.UserName
            && x.Password == model.Password && x.IsActive == 1 && x.IsExpired == 0 && x.IsLocked == 0);

            string responseMessage = string.Empty;

            if (response != null)
            {
                HttpContext.Session.SetInt32("StudentId", response.StudentId);
                HttpContext.Session.SetInt32("EmployeeId", response.EmployeeId);
                HttpContext.Session.SetInt32("UserId", response.Id);
                HttpContext.Session.SetInt32("SessionId", model.SessionId);
                if (response.StudentId != 0)
                {
                    //Populate session with student Information
                    await GetStudentInfo(response.StudentId);
                }
                else if(response.EmployeeId!=0)
                {
                    //Populate session with student Information
                    await GetEmployeeInfo(response.EmployeeId);
                }

                if (response.IsExpired == 1)
                {
                    responseMessage = "User expired. please contact admin";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else if (response.IsLocked == 1)
                {
                    responseMessage = "User locked. please contact admin";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else if (response.Attempt > 3)
                {
                    responseMessage = "User attempted more than 3 times , your account has been locked";
                    return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
                }
                else
                {
                    return await ValidatedUser(response);
                }

            }
            responseMessage = "Invalid user name and password.";
            return RedirectToAction("Login", "Authenticate", new { message = responseMessage });
        }

        private async Task<IActionResult> ValidatedUser(Authenticate model)
        {
            HttpContext.Session.SetString("userName", model.UserName);
            var lastLogin = model.LastLoginDateTime;
            model.LastLoginDateTime = DateTime.Now;

            await _IAuthenticateRepo.CreateNewContext();

            var responseData = await _IAuthenticateRepo.Update(model);
            HttpContext.Session.SetString("UserName", model.UserName);

            HttpContext.Session.SetObject("menuSubMenu", await GetMenuSubMenu(model.RoleId));
            HttpContext.Session.SetInt32("EmployeeId", model.EmployeeId);

            return RedirectToAction("Index", "Home");
        }

        private async Task<List<MenuSubMenuVm>> GetMenuSubMenu(int roleId)
        {
            HttpContext.Session.SetInt32("RoleId", roleId);
            var result = (from UR in await _IUserAccessRight.GetList(x => x.RoleId == roleId && x.IsActive == 1)
                          join MM in await _IModuleRepo.GetList(x => x.IsActive == 1)
                          on UR.ModuleId equals MM.Id
                          join SM in await _ISubModuleRepo.GetList(x => x.IsActive == 1)
                          on UR.SubModuleId equals SM.Id
                          select new MenuSubMenuVm
                          {
                              ModuleId = MM.Id,
                              ModuleName = MM.ModuleName,
                              ModuleClass = MM.ClassName,
                              SubModuleId = SM.Id,
                              SubModuleName = SM.SubModuleName,
                              SubModuleClass = SM.ClassName,
                              ActionName = SM.ActionName,
                              ControllerName = SM.ControllerName,
                              ModuleClassName = MM.ClassName,
                              ModuleDisplayOrder = MM.DisplayOrder,
                              SubModuleDisplayOrder = SM.DisplayOrder
                          }).ToList();

            return result.OrderBy(x => x.ModuleDisplayOrder).ThenBy(x => x.SubModuleDisplayOrder).ToList();

        }

        private async Task GetStudentInfo(int studentId)
        {
            var model = await _studentPromoteRepo.GetSingle(x => x.IsActive == 1 && x.StudentId == studentId);
            var studentModel = await _studentMasterRepo.GetSingle(x => x.Id == studentId);
            StudentAccountModel accountModel = new StudentAccountModel()
            {
                BatchId = model.BatchId,
                CourseId = model.CourseId,
                StudentId = model.StudentId,
                ImagePath= studentModel.StudentPhoto
                
            };
            HttpContext.Session.SetObject("StudentInfo",accountModel);
        }
        private async Task GetEmployeeInfo(int userId)
        {
            var model = await _employeeRepo.GetSingle(x=>x.Id==userId);
            StudentAccountModel accountModel = new StudentAccountModel()
            {
                BatchId =0,
                CourseId =0,
                StudentId = model.Id,
                ImagePath = model.Photo

            };
            HttpContext.Session.SetObject("StudentInfo", accountModel);
        }

        public async Task<IActionResult> SendLoginCredential(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                return Json("User name does not exists.");
            }
            var model = await _IAuthenticateRepo.GetSingle(x => x.UserName.Trim().ToLower() == userName.ToLower().Trim());
            string email = string.Empty;
            if(model==null)
            {
                return Json("User name does not exists.");
            }
            if(model.StudentId>0)
            {
                email = (await _studentMasterRepo.GetSingle(x => x.Id == model.StudentId)).StudentEmail;
            }
            else if(model.EmployeeId>0)
            {
                email = (await _employeeRepo.GetSingle(x => x.Id == model.EmployeeId)).Email;
            }
            if (!string.IsNullOrEmpty(email))
            {
                new SendEmailNotification().SendCustomEmail(userName, email, "Password reset request for SERP Portal", await GetHtmlHelperEmail(userName, model.Password));
                return Json("Password has been reset.Please check your email.");
            }
            else {
                return Json("User name does not exists.");
            }

        }


        public async Task<string> GetHtmlHelperEmail(string userName, string password)
        {
            string emailBody = @"Dear Candidate "+ userName +" your password has been reset please login to the portal with Password :"+ password;
            return await Task.Run(() => emailBody);
        }
        
    }
}