using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.HRModule
{
    public class SalarySlipController : Controller
    {
        private readonly ISalaryHeadRepo _salaryHeadRepo;
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _employeeRepo;
        private readonly IGenericRepository<InstituteMaster, int> _instituteRepo;

        public SalarySlipController(ISalaryHeadRepo salaryHeadRepo, IGenericRepository<EmployeeBasicInfoModel, int> employeeRepo, IGenericRepository<InstituteMaster, int>  instituteRepo)
        {
            _salaryHeadRepo = salaryHeadRepo;
            _employeeRepo = employeeRepo;
            _instituteRepo = instituteRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.employeeList = await _employeeRepo.GetList(x => x.IsActive == 1);
            return await Task.Run(() => PartialView("~/Views/HRModule/_SalarySlipPartial.cshtml")); 
        }

        [HttpGet]
        public async Task<IActionResult> SalarySlipDetails(int employeeId, int year, int month)
        {
            var instituteModel = await _instituteRepo.GetSingle(x => x.IsActive == 1);
            List<EmployeeSalarySlip> models = new List<EmployeeSalarySlip>();
          

            var DateTime = new DateTime(year, month, 1);
            models = await _salaryHeadRepo.GetEmployeeSalarySlip(month, employeeId,DateTime);
            models.First().InstituteMaster = instituteModel;
            return PartialView("~/Views/HRModule/_EmployeeSalarySlipPartial.cshtml", models);
        }

        public async Task<IActionResult> SalaryStatment()
        {
            var responseData = await _salaryHeadRepo.GetSalaryStatement(7, 31);
            return PartialView("~/Views/HRModule/_salaryStatement.cshtml", responseData);
        }
    }
}