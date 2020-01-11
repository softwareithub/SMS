using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.HRModule
{
    public class EmployeeController : Controller
    {
        private readonly IGenericRepository<EmployeeBasicInfoModel, int> _basicInfoRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGenericRepository<DepartmentModel, int> _departmentRepo;
        private readonly IGenericRepository<DesignationModel, int> _designationRepo;
        private readonly IGenericRepository<BranchInfoModel, int> _branchInfoRepo;
        private readonly IGenericRepository<EmployeeSalaryModel, int> _employeeSalaryRepo;
        private readonly IGenericRepository<PayHeadesModel, int> _payHeadRepo;


        public EmployeeController(IGenericRepository<EmployeeBasicInfoModel, int> basicInfoRepo,
            IHostingEnvironment hostingEnvironment, IGenericRepository<DepartmentModel, int> departmentRepo,
            IGenericRepository<DesignationModel, int> designationRepo,
            IGenericRepository<BranchInfoModel, int> branchInfoRepo,
            IGenericRepository<EmployeeSalaryModel, int> salaryRepo,
            IGenericRepository<PayHeadesModel, int> payHeadRepo
            )
        {
            _basicInfoRepo = basicInfoRepo;
            _hostingEnvironment = hostingEnvironment;
            _departmentRepo = departmentRepo;
            _designationRepo = designationRepo;
            _branchInfoRepo = branchInfoRepo;
            _employeeSalaryRepo = salaryRepo;
            _payHeadRepo = payHeadRepo;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView("~/Views/HRModule/_EmployeeInfoIndex.cshtml"));
        }

        public async Task<IActionResult> BasicInfo(int id)
        {
            ViewBag.Departments = await _departmentRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            ViewBag.Designations = await _designationRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            ViewBag.BankInfo = await _branchInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

            var model = await _basicInfoRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/HRModule/_BasicInfoCreatePartial.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBasicInfo(EmployeeBasicInfoModel model, IFormFile Photo)
        {

            List<IFormFile> formFiles = new List<IFormFile>();
            formFiles.Add(Photo);

            var imagePaths = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnvironment);

            if (imagePaths.Count() == 0)
            {
                model.Photo = string.Empty;
                model.Photo = string.Empty;
            }
            else
            {
                model.Photo = string.IsNullOrEmpty(model.Photo) ? imagePaths[0] : model.Photo;
                model.Photo = string.IsNullOrEmpty(model.Photo) ? imagePaths[1] : model.Photo;
            }


            if (model.Id > 0)
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _basicInfoRepo.Update(model);
                var json = new { BasicInfoId = model.Id, resultStatus = ResponseData.Instance.GenericResponse(result) };
                return Json(json);
            }
            else
            {
                var result = await _basicInfoRepo.CreateEntity(model);
                await _basicInfoRepo.CreateNewContext();
                var basicId = (await _basicInfoRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0)).Max(x => x.Id);
                var json = new { BasicInfoId = basicId, resultStatus = ResponseData.Instance.GenericResponse(result) };
                return Json(json);
            }
        }

        public async Task<IActionResult> CreatePreviousQual(int id)
        {
            return await Task.Run(() => PartialView("~/Views/HRModule/_PreviousQualificationPartial.cshtml"));
        }

        public async Task<IActionResult> EmployeeSalary(int id)
        {
            List<EmployeeSalaryVm> models = new List<EmployeeSalaryVm>();
            var empSalModels = await _employeeSalaryRepo.GetList(x => x.EmployeeId == id && x.IsDeleted == 0 && x.IsActive == 1);
            var payHeads = await _payHeadRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            if (empSalModels.Count() == 0)
            {
                payHeads.ToList().ForEach(item =>
                {
                    EmployeeSalaryVm model = new EmployeeSalaryVm()
                    {
                        Amount = decimal.Parse("0.0"),
                        SalaryHead = item.Id,
                        HeadName= item.Name,
                        AdditionDeduction= item.Addition_Deduction
                    };
                    models.Add(model);
                });

                return PartialView("~/Views/HRModule/_EmployeeSalaryPartial.cshtml", models);
            }
            else
            {
                var result = (from empSal in empSalModels
                              join PH in payHeads
                              on empSal.HeadId equals PH.Id
                              where empSal.IsActive == 1 && empSal.IsDeleted == 0
                              && PH.IsDeleted == 0 && PH.IsActive == 1
                              select new EmployeeSalaryVm
                              {
                                  EmployeeId = empSal.EmployeeId,
                                  Amount = empSal.Amount,
                                  SalaryHead = PH.Id,
                                  HeadName = PH.Name,
                                  EmployeeSalaryId = empSal.Id,
                                  AdditionDeduction=PH.Addition_Deduction

                              }).ToList();
                return PartialView("~/Views/HRModule/_EmployeeSalaryPartial.cshtml", result);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeSalary()
        {
            var amount = Request.Form["data.Amount"];
            var payHeadId = Request.Form["data.SalaryHead"];
            var employeeId = Convert.ToInt32(Request.Form["EmployeeId"]);

            var employeeSal = await _employeeSalaryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.EmployeeId == employeeId);
            employeeSal.ToList().ForEach(item => {
                item.IsDeleted = 1;
                item.IsActive = 0;
            });
            await _employeeSalaryRepo.CreateNewContext();

            var updateResult = await _employeeSalaryRepo.Delete(employeeSal.ToArray());
            await _employeeSalaryRepo.CreateNewContext();

            List<EmployeeSalaryModel> models = new List<EmployeeSalaryModel>();
            for(int i=0; i< payHeadId.Count(); i++)
            {
                EmployeeSalaryModel model = new EmployeeSalaryModel() {
                    EmployeeId = employeeId,
                    HeadId = Convert.ToInt32(payHeadId[i]),
                    Amount = Convert.ToDecimal(amount[i]),
                    IsActive=1,
                    IsDeleted=0
                };
                models.Add(model);
            }
            var result = await _employeeSalaryRepo.Add(models.ToArray());
            return await Task.Run(() => Json(ResponseData.Instance.GenericResponse(result)));
        }

        public async Task<IActionResult> GetEmployeeList()
        {
            var result = await _basicInfoRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            return PartialView("~/Views/HRModule/_EmploeeInfoPartial.cshtml", result);
        }
    }
}