using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentFeeDetailController : ControllerBase
    {

        private readonly IFeeDetailRepo _feeDetailRepo;
        private readonly IQuickSearchRepo _QuickSearchRepo;
        public StudentFeeDetailController(IFeeDetailRepo feeDetailRepo, IQuickSearchRepo quickRepo) {
            _feeDetailRepo = feeDetailRepo;
            _QuickSearchRepo = quickRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentDetail(int studentId)
        {
            var result = await _feeDetailRepo.GetFeeDetailRepo(studentId);
            result.ForEach(x =>
            {
                switch (x.FeePaymentType)
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
            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetStudentFeeDeposit(int studentId)
        {
            var model = await _QuickSearchRepo.StudentFeeDetailReport(studentId);
            return Ok(model);

        }
    }
}
