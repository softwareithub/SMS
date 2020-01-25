using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.FeeDetails;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class StudentFeeDetailController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentMasterRepo;
        public readonly IGenericRepository<StudentPromote, int> _IStudentPromote;
        private readonly IGenericRepository<FeeDetailClassWise, int> _feeClassWiseDetailRepo;
        private readonly IGenericRepository<FeeDiscountParticularWiseModel, int> _feeDiscountParticularRepo;
        private readonly IGenericRepository<FeeDiscountModel, int> _feeDsicountRepo;
        private readonly IFeeDetailRepo _feeDetailRepo;
        public StudentFeeDetailController(IGenericRepository<StudentMaster, int> studentMasterRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo,
            IGenericRepository<FeeDetailClassWise, int> feeDetailClassWiseRepo,
            IGenericRepository<FeeDiscountParticularWiseModel, int> _feeCategoryRepo,
            IGenericRepository<FeeDiscountModel, int> feeDiscountRepo,
            IFeeDetailRepo feeDetailRepo
            )
        {
            _studentMasterRepo = studentMasterRepo;
            _IStudentPromote = studentPromoteRepo;
            _feeClassWiseDetailRepo = feeDetailClassWiseRepo;
            _feeDiscountParticularRepo = _feeCategoryRepo;
            _feeDsicountRepo = feeDiscountRepo;
            _feeDetailRepo = feeDetailRepo;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView("~/Views/FeeTransaction/_StudentFeeDetailIndexPartial.cshtml"));
        }

        public async Task<IActionResult> GetStudentDetail(string name, string phone, string rollCode, string registration)
        {
            List<StudentMaster> modelList = (await _studentMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0)).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                modelList = modelList.Where(x => x.Name.Trim().ToLower().StartsWith(name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(phone))
            {
                modelList = modelList.Where(x => x.StudentPhone.ToLower().Trim().StartsWith(phone.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(rollCode))
            {
                modelList = modelList.Where(x => x.RollCode.ToLower().Trim().StartsWith(rollCode.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(registration))
            {
                modelList = modelList.Where(x => x.RegistrationNumber.Trim().ToLower().StartsWith(registration.ToLower().Trim())).ToList();
            }

            return PartialView("~/Views/FeeTransaction/StudentInfoForFeeDetailPartial.cshtml", modelList);
        }

        public async Task<IActionResult> GetFeeDetail(int studentId)
        {
            var result = await _feeDetailRepo.GetFeeDetailRepo(studentId);

            result.ForEach(x =>
            {
                switch(x.FeePaymentType)
                {
                    case "OT":
                        x.FeePaymentType = "One Time";
                        break;
                    case "YL":
                        x.FeePaymentType = "Yearly";
                        break;
                    case "HY":
                        x.FeePaymentType = "Half Yearly";
                        break;
                    case "QY":
                        x.FeePaymentType = "Quaterly";
                        break;
                    case "MT":
                        x.FeePaymentType = "Monthly";
                        break;
                }
                if (x.DependentOnParticular == 1)
                {
                    if (x.DiscountType.ToLower().Trim() == "per")
                        x.NetAmount = x.Amount - ((x.Amount * x.PertDiscountValue) / 100);
                    else
                        x.NetAmount = x.Amount - x.PertDiscountValue;
                }
                else
                {
                    if (x.DiscountType.ToLower().Trim() == "per")
                        x.NetAmount = x.Amount - ((x.Amount * x.DiscountValue) / 100);
                    else
                        x.NetAmount = x.Amount - x.DiscountValue;
                }
            });
            return PartialView("~/Views/FeeTransaction/_StudentFeeDetails.cshtml", result);
        }
    }
}