using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.HRModule;
using SERP.Core.Model.HRModel;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.BlobUtility;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

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
        public async Task<IActionResult> Index(int id)
        {
            EmployeeInfoVm model = new EmployeeInfoVm();
            //populate the ViewBag for Select List items
            ViewBag.DepartmentDetails = await _departmentRepo.GetList(x => x.IsActive == 1);
            ViewBag.Designation = await _designationRepo.GetList(x => x.IsActive == 1);
            ViewBag.BankDetails = await _branchInfoRepo.GetList(x => x.IsActive == 1);


            // Get Employee info by Id
            var employeeModel = await _basicInfoRepo.GetSingle(x => x.Id == id);
            model.EmployeeBasicInfoModel = employeeModel;

            List<EmployeeSalaryDetailModel> employeeSalaryModels = new List<EmployeeSalaryDetailModel>();

            //Get All  Pay head Details irrespective of employee
            (await _payHeadRepo.GetList(x => x.IsActive == 1)).ToList().ForEach(item =>
            {
                EmployeeSalaryDetailModel model = new EmployeeSalaryDetailModel();
                model.Id = item.Id;
                model.HeadName = item.Name;
                model.HeadId = item.Id;
                model.IsDependentOnDay = item.IsDependentPerDay;
                model.AdditionDeduction = item.Addition_Deduction;
                employeeSalaryModels.Add(model);
            });

            // Get employee salary details
            var employeeSalaryDetails = await _employeeSalaryRepo.GetList(x => x.EmployeeId == id);

            //Populate the employee salary with head name respectively
            if (employeeSalaryDetails.Count() > 0)
            {
                employeeSalaryModels.ForEach(item =>
                {
                    item.Amount = employeeSalaryDetails.ToList().Find(x => x.HeadId == item.Id).Amount;
                });
            }
            //populate the viewModel with the List
            model.EmployeeSalaryModels = employeeSalaryModels;
            return PartialView("~/Views/HRModule/_EmployeeInfoIndex.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeInfoVm model, IFormFile employeePhoto)
        {
            if(model.EmployeeBasicInfoModel.Id==0)
            {

                List<IFormFile> formFiles = new List<IFormFile>();
                formFiles.Add(employeePhoto);
                var imagePaths = await UploadImage.UploadImageOnFolder(formFiles, _hostingEnvironment);
                model.EmployeeBasicInfoModel.Photo = imagePaths.First();
                //Insert data into employeeBasic Information
                var basicInfoResponse = await _basicInfoRepo.CreateEntity(model.EmployeeBasicInfoModel);
                //Create new Context
                await _basicInfoRepo.CreateNewContext();
                //Get Last Inserted Employee
                var employeeId = (await _basicInfoRepo.GetList(x => x.IsActive == 1)).Max(x => x.Id);
                //Check employee inserted successfully
                if (basicInfoResponse == Utilities.ResponseUtilities.ResponseStatus.AddedSuccessfully)
                {
                    List<EmployeeSalaryModel> salaryModels = new List<EmployeeSalaryModel>();
                    model.EmployeeSalaryModels.ToList().ForEach(item =>
                    {
                        EmployeeSalaryModel salaryModel = new EmployeeSalaryModel();
                        salaryModel.Amount = item.Amount;
                        salaryModel.HeadId = item.HeadId;
                        salaryModel.EmployeeId = employeeId;
                        salaryModels.Add(salaryModel);
                    });
                    var salaryResponse = await _employeeSalaryRepo.Add(salaryModels.ToArray());
                    return Json(ResponseData.Instance.GenericResponse(salaryResponse));
                }
            }
            else
            {
                var response = await UpdateEmplolyeeInfo(model, employeePhoto);
                return Json(ResponseData.Instance.GenericResponse(response));
            }

            return Json("Error in employee creation Please contact admin department.");
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeList()
        {
            var employeeDetails = await _basicInfoRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/HRModule/_EmployeeListPartial.cshtml", employeeDetails);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            // Get employee detail by id
            // sey
            var employeeDetail = await _basicInfoRepo.GetSingle(x => x.Id == id);
            employeeDetail.IsActive = 0;
            employeeDetail.IsDeleted = 1;

            await _basicInfoRepo.CreateNewContext();
            var responseBasicInfo = await _basicInfoRepo.Update(employeeDetail);
            var salaryDetails = await _employeeSalaryRepo.GetList(x => x.EmployeeId == id);

            salaryDetails.ToList().ForEach(item => {
                item.IsActive = 0;
                item.IsDeleted = 1;
            });

            await _employeeSalaryRepo.CreateNewContext();

            var deleteSalaryResponse = await _employeeSalaryRepo.Update(salaryDetails.ToArray());
            return Json(ResponseData.Instance.GenericResponse(deleteSalaryResponse));
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeSalaryInfo(int empId)
        {
            var employeeDetail = await _basicInfoRepo.GetSingle(x => x.Id == empId);
            ViewBag.EmployeeName = employeeDetail.Name + " (" + employeeDetail.EmpCode + ")";
            List<EmployeeSalaryDetailModel> employeeSalaryDetailModels = new List<EmployeeSalaryDetailModel>();
            employeeSalaryDetailModels = (from ESI in await _employeeSalaryRepo.GetList(x => x.IsActive == 1 && x.EmployeeId==empId)
                                          join PM in await _payHeadRepo.GetList(x => x.IsActive == 1)
                                          on ESI.HeadId equals PM.Id
                                          select new EmployeeSalaryDetailModel
                                          {
                                              HeadId= PM.Id,
                                              HeadName= PM.Name,
                                              Amount= ESI.Amount,
                                              AdditionDeduction= PM.Addition_Deduction,
                                              IsDependentOnDay= PM.IsDependentPerDay
                                          }).ToList();

            return PartialView("~/Views/HRModule/_EmployeeSalaryDetailPartial.cshtml", employeeSalaryDetailModels);
        }

        private async Task<ResponseStatus> UpdateEmplolyeeInfo(EmployeeInfoVm model, IFormFile employeePhoto)
        {
            var updateEmployeeBasicResponse = await _basicInfoRepo.Update(model.EmployeeBasicInfoModel);
            //if update successfull then update the employee  salary
            var employeeSalary = await _employeeSalaryRepo.GetList(x => x.EmployeeId == model.EmployeeBasicInfoModel.Id);
            if(employeeSalary.Count()>0)
            {
                employeeSalary.ToList().ForEach(item => {
                    model.EmployeeSalaryModels.ToList().ForEach(sal => {
                        if (item.HeadId == sal.HeadId)
                        {
                            item.Amount = sal.Amount;
                        }
                    });
                });
                await _employeeSalaryRepo.CreateNewContext();
                //update employee Salary
                var salaryResponse = await _employeeSalaryRepo.Update(employeeSalary.ToArray());
                return salaryResponse;
            }
            else
            {
                List<EmployeeSalaryModel> salaryModels = new List<EmployeeSalaryModel>();
                model.EmployeeSalaryModels.ToList().ForEach(item => {
                    EmployeeSalaryModel salmodel = new EmployeeSalaryModel();
                    salmodel.Amount = item.Amount;
                    salmodel.HeadId = item.HeadId;
                    salmodel.EmployeeId = model.EmployeeBasicInfoModel.Id;
                    salmodel.IsActive = 1;
                    salmodel.IsDeleted = 0;
                    salaryModels.Add(salmodel);
                });
                await _employeeSalaryRepo.CreateNewContext();
                var salaryResponse = await _employeeSalaryRepo.Update(salaryModels.ToArray());
                return salaryResponse;
            }

           
        }
    }
}