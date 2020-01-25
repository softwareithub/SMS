using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

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
            FeeDetailModel model = new FeeDetailModel();
            List<FeeDetailClassWise> models = new List<FeeDetailClassWise>();
            var courseIdints = (courseIds.Split(',')).Select(x => Int32.Parse(x)).ToList();
            var result = await _feeCategoryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            var feeAmount = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && courseIdints.Contains(x.ClassId));
            result.ToList().ForEach(item =>
            {
                var model = new FeeDetailClassWise()
                {
                    CategoryId = item.Id,
                    CategoryName = item.Name,
                    Amount = feeAmount.Count() > 0 ? feeAmount.First(x => x.CategoryId == item.Id).Amount : decimal.Parse("0.0"),
                    FeePaymentType = feeAmount.Count() > 0 ? feeAmount.First(x => x.CategoryId == item.Id).FeePaymentType : string.Empty,
                };
                models.Add(model);
            });

            HttpContext.Session.SetString("CourseId", courseIds);
            model.FeeDetailModels = models;
            return await Task.Run(() => PartialView("~/Views/FeeTransaction/FeeParticualrPartial.cshtml", model));
        }

        public async Task<IActionResult> AllocateStudentFee(FeeDetailModel modelData)
        {
            var courseIds = (HttpContext.Session.GetString("CourseId").ToString().Split(',')).Select(x => Int32.Parse(x)).ToList();
            ResponseStatus result =ResponseStatus.AddedSuccessfully ;
            var maxFeeType = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            int feeTypeId = maxFeeType.Count() != 0 ? maxFeeType.Max(x => x.Type) + 1 : 1;

            await _feeClassWiseDetailRepo.CreateNewContext();

            var deletePreviousFee = await _feeClassWiseDetailRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && courseIds.Contains(x.ClassId));
            deletePreviousFee.ToList().ForEach(x => {
                x.IsActive = 0;
                x.IsDeleted = 1;
            });

            await _feeClassWiseDetailRepo.CreateNewContext();

            var deleteresult = await _feeClassWiseDetailRepo.Update(deletePreviousFee.ToArray());

            for (int i=0; i< courseIds.Count(); i++)
            {
                modelData.FeeDetailModels.ToList().ForEach(x =>
                {
                    x.Type = feeTypeId;
                    x.ClassId =Convert.ToInt32(courseIds[i]);
                    x.Id = 0;
                });
                await _feeClassWiseDetailRepo.CreateNewContext();
                result = await _feeClassWiseDetailRepo.Add(modelData.FeeDetailModels.ToArray());
            }
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