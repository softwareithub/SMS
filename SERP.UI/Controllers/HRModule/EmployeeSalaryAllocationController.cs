using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.HRModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class EmployeeSalaryAllocationController : Controller
    {
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<EmployeeSalaryModel, int> _employeeSalaryRepo;
        private readonly IGenericRepository<PayHeadesModel, int> _payHeadRepo;

        public EmployeeSalaryAllocationController(IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo,
            IGenericRepository<EmployeeSalaryModel, int> empSalaryRepo, IGenericRepository<PayHeadesModel, int> payHeadRepo)
        {
            _employeeRepo = employeeRepo;
            _employeeSalaryRepo = empSalaryRepo;
            _payHeadRepo = payHeadRepo;
        }
        public async Task<IActionResult> Index()
        {
            var employeeDetail = await _employeeRepo.GetList(x => x.IsActive == 1);
            employeeDetail.ToList().ForEach(item =>
            {
                item.Name = item.Name + " (" + item.EmpCode + " )";
            });
            ViewBag.employeeList = employeeDetail;
            return PartialView("~/Views/HRModule/_EmployeeSalaryAllocationPartial.cshtml");
        }

        public async Task<IActionResult> GetEmployeeSalary(int empId)
        {
            List<EmployeeSalaryDetailModel> employeeSalaryDetailModels = new List<EmployeeSalaryDetailModel>();
            employeeSalaryDetailModels = (from ESI in await _employeeSalaryRepo.GetList(x => x.IsActive == 1 && x.EmployeeId == empId)
                                          join PM in await _payHeadRepo.GetList(x => x.IsActive == 1)
                                          on ESI.HeadId equals PM.Id
                                          select new EmployeeSalaryDetailModel
                                          {
                                              HeadId = PM.Id,
                                              HeadName = PM.Name,
                                              Amount = ESI.Amount,
                                              AdditionDeduction = PM.Addition_Deduction,
                                              IsDependentOnDay = PM.IsDependentPerDay
                                          }).ToList();

            employeeSalaryDetailModels.ForEach(item => {
                item.EmployeeId = empId;
            });

            return PartialView("~/Views/HRModule/EmployeeSalaryPartial.cshtml", employeeSalaryDetailModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeSalary(string[] Amount, string[] HeadId, int employeeId)
        {
            List<EmployeeSalaryModel> salModels = new List<EmployeeSalaryModel>();
            var models = await _employeeSalaryRepo.GetList(x => x.EmployeeId == employeeId && x.IsActive==1 && x.IsDeleted==0);
            for (int i = 0; i < HeadId.Count(); i++)
            {
                EmployeeSalaryModel model = new EmployeeSalaryModel();
                model = models.ToList().Where(x => x.HeadId == Convert.ToInt32(HeadId[i])).FirstOrDefault();
                model.Amount = Convert.ToDecimal(Amount[i]);
                salModels.Add(model);

            }
            await _employeeSalaryRepo.CreateNewContext();
            var response = await _employeeSalaryRepo.Update(salModels.ToArray());
            return Json(ResponseData.Instance.GenericResponse(response));
        }
    }
}