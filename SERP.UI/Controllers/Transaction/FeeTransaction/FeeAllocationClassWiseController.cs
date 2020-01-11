using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class FeeAllocationClassWiseController : Controller
    {
        private readonly IGenericRepository<FeeDetailClassWise, int> _feeClassWiseDetailRepo;
        private readonly IGenericRepository<CourseMaster, int> _CourseMasterRepo;
        private readonly IGenericRepository<FeeCategory, int> _feeCategoryRepo;
        public FeeAllocationClassWiseController(IGenericRepository<FeeDetailClassWise, int> feeClassWiseDetailRepo
            , IGenericRepository<CourseMaster, int> courseMasterRepo, IGenericRepository<FeeCategory, int> feeCategoryRepo)
        {
            _feeClassWiseDetailRepo = feeClassWiseDetailRepo;
            _CourseMasterRepo = courseMasterRepo;
            _feeCategoryRepo = feeCategoryRepo;
        }
        public async Task<IActionResult> ClassWiseFeeAllocation()
        {
            var courseDetails = await _CourseMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var mappedCoursed = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            courseDetails.ToList().ForEach(x =>
            {
                mappedCoursed.ToList().ForEach(item =>
                {
                    if (x.Id == item.ClassId)
                        x.IsMapped = true;
                });
            });
            ViewBag.CourseList = courseDetails;
            return await Task.Run(() => PartialView("~/Views/FeeTransaction/_ClassWiseFeeAllocation.cshtml"));
        }

        public async Task<IActionResult> GetFeeParticularPartial(string courseIds)
        {
            List<FeeDetailClassWise> models = new List<FeeDetailClassWise>();
            var result = await _feeCategoryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            result.ToList().ForEach(item =>
            {
                var model = new FeeDetailClassWise()
                {
                    CategoryId = item.Id,
                    CategoryName = item.Name,
                    Amount = decimal.Parse("0.0"),
                    FeePaymentType = "MT"
                };
                models.Add(model);
            });

            TempData["CourseId"] = courseIds;
            return await Task.Run(() => PartialView("~/Views/FeeTransaction/FeeParticualrPartial.cshtml", models));
        }

        public async Task<IActionResult> AllocateStudentFee()
        {
            var categoryIds = Request.Form["CategoryId"];
            var feeAmounts = Request.Form["feeAmount"];
            var feeType = Request.Form["feeType"];
            var courseIds = TempData["CourseId"].ToString().Split(',');
            List<FeeDetailClassWise> models = new List<FeeDetailClassWise>();
            for (int k = 0; k < courseIds.Count(); k++)
            {
                int courseId = Convert.ToInt32(courseIds[k]);
                for (int i = 0; i < categoryIds.Count(); i++)
                {
                    FeeDetailClassWise model = new FeeDetailClassWise()
                    {
                        Amount = Convert.ToDecimal(feeAmounts[i]),
                        CategoryId = Convert.ToInt32(categoryIds[i]),
                        FeePaymentType = feeType[i].ToString(),
                        IsActive = 1,
                        IsDeleted = 0,
                        CreatedDate = DateTime.Now.Date,
                        CreatedBy = 1,
                        ClassId = courseId
                    };
                    models.Add(model);
                }
            }
            var maxFeeType = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            int feeTypeId = maxFeeType.Count() != 0 ? maxFeeType.Max(x => x.Type) + 1 : 1;

            models.ToList().ForEach(x =>
            {
                x.Type = feeTypeId;
            });
            await _feeClassWiseDetailRepo.CreateNewContext();
            var result = await _feeClassWiseDetailRepo.Add(models.ToArray());

            return Json(ResponseData.Instance.GenericResponse(result));
        }

        public async Task<IActionResult> GetFeeDetail(string courseIds)
        {
            var courseModels = await _CourseMasterRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var feecategorieModels = await _feeCategoryRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
            var feeDetailModels = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            int[] courseIdArray = Array.ConvertAll(courseIds.Split(','), z => int.Parse(z));

            var modelsList = feeDetailModels.Where(z => courseIdArray.ToList().Any(x => z.ClassId == x)).Select(x => new
            {
                x.CategoryId,
                x.ClassId,
                x.Amount,
                x.FeePaymentType,
                x.Type
            }).ToList();

            var result = (from FD in modelsList
                          join FC in feecategorieModels
                          on FD.CategoryId equals FC.Id
                          join CM in courseModels
                          on FD.ClassId equals CM.Id
                          select new ClassFeeAllocationVm
                          {
                              CategoryName = FC.Name,
                              CategoryCode = FC.Code,
                              ClassName = CM.Name,
                              Amount = FD.Amount,
                              PaymentType = FD.FeePaymentType,
                              Type = FD.Type
                          }).ToList();
            return PartialView("~/Views/FeeTransaction/_ClassWiseFeeAllocationListPartial.cshtml", result);
        }
    }
}