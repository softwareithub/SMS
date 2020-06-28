using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
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
        private readonly IGenericRepository<CourseMaster, int> _ICourseRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;
        private readonly IFeeDetailRepo _feeDetailRepo;
        public StudentFeeDetailController(IGenericRepository<StudentMaster, int> studentMasterRepo,
            IGenericRepository<StudentPromote, int> studentPromoteRepo,
            IGenericRepository<FeeDetailClassWise, int> feeDetailClassWiseRepo,
            IGenericRepository<FeeDiscountParticularWiseModel, int> _feeCategoryRepo,
            IGenericRepository<FeeDiscountModel, int> feeDiscountRepo,
            IFeeDetailRepo feeDetailRepo, IGenericRepository<BatchMaster, int> _batchRepo,
            IGenericRepository<CourseMaster, int> courseRepo

            )
        {
            _studentMasterRepo = studentMasterRepo;
            _IStudentPromote = studentPromoteRepo;
            _feeClassWiseDetailRepo = feeDetailClassWiseRepo;
            _feeDiscountParticularRepo = _feeCategoryRepo;
            _feeDsicountRepo = feeDiscountRepo;
            _feeDetailRepo = feeDetailRepo;
            _ICourseRepo = courseRepo;
            _IBatchRepo = _batchRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseList = await _ICourseRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return await Task.Run(() => PartialView("~/Views/FeeTransaction/_StudentFeeDetailIndexPartial.cshtml"));
        }

        public async Task<IActionResult> GetStudentDetail(int courseId,int batchId)
        {
            List<StudentMaster> modelList = (await _studentMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.CourseId==courseId && x.BatchId==batchId)).ToList();
            return Json(modelList);
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
                        x.YearlyAmount = x.Amount;
                        break;
                    case "YL":
                        x.FeePaymentType = "Yearly";
                        x.YearlyAmount = x.Amount;
                        break;
                    case "HY":
                        x.FeePaymentType = "Half Yearly";
                        x.YearlyAmount = x.Amount * 2;
                        break;
                    case "QY":
                        x.FeePaymentType = "Quaterly";
                        x.YearlyAmount = x.Amount * 3;
                        break;
                    case "MT":
                        x.FeePaymentType = "Monthly";
                        x.YearlyAmount = x.Amount * 12;
                        break;
                }
                if (x.DependentOnParticular == 1)
                {
                    if (x.PertDiscountType.ToLower().Trim() == "per")
                        x.NetAmount = x.YearlyAmount - ((x.YearlyAmount * x.PertDiscountValue) / 100);
                    else
                        x.NetAmount = x.YearlyAmount - x.PertDiscountValue;
                }
                else
                {
                    if (x.DiscountType?.ToLower()?.Trim() == "per")
                        x.NetAmount = x.YearlyAmount - ((x.YearlyAmount * x.DiscountValue) / 100);
                    else
                        x.NetAmount = x.YearlyAmount;
                }
            });
            return PartialView("~/Views/FeeTransaction/_StudentFeeDetails.cshtml", result);
        }
    }
}