using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Entities.UserManagement;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.UserManagement
{
    public class UserLoginController : Controller
    {
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _IEmployeeRepo;
        private readonly IGenericRepository<Authenticate, int> _IAuthenticate;
        private readonly IGenericRepository<StudentMaster, int> _IStudentRepo;
       

        public UserLoginController(IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<Authenticate, int> authenticateRepo, IGenericRepository<StudentMaster, int> studentRepo)
        {
            _IEmployeeRepo = employeeRepo;
            _IAuthenticate = authenticateRepo;
            _IStudentRepo = studentRepo;
         
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Employee = await _IEmployeeRepo.GetList(x => x.IsActive == 1);
            ViewBag.StudentList = await _IStudentRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/UserManagement/_UserLoginCreatePartial.cshtml",await _IAuthenticate.GetSingle(x=>x.Id==id));
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
            var result = (from AM in await _IAuthenticate.GetList(x => x.IsActive == 1)
                          join EM in await _IEmployeeRepo.GetList(x => x.IsActive == 1)
                          on AM.EmployeeId equals EM.Id
                          select new UserLoginModel
                          {
                                Id= AM.Id,
                                EmployeeName= EM.Name,
                                UserName= AM.UserName,
                                Password= AM.Password,
                                IsExpired= AM.IsExpired,
                                IsActive= AM.IsActive,
                                IsLocked= AM.IsLocked,
                                EmployeeCode= EM.EmpCode
                          }).ToList();
            return PartialView("~/Views/UserManagement/_UserManagementList.cshtml", result);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _IAuthenticate.GetSingle(x => x.Id == id);
            var deleteModel= Utilities.CommanHelper.CommanDeleteHelper.CommanDeleteCode(model,1);
            await _IAuthenticate.CreateNewContext();
            var response = await _IAuthenticate.Update(deleteModel);
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}