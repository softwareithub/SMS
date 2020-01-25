using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Core.Model.TransactionViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Transaction.FeeTransaction
{
    public class FeeDiscountController : Controller
    {
        private readonly IGenericRepository<FeeDiscountParticularWiseModel, int> _feeDiscountParticularRepo;
        private readonly IGenericRepository<FeeDiscountModel, int> _feeDsicountRepo;
        private readonly IGenericRepository<FeeCategory, int> _feeCategoryRepo;

        public FeeDiscountController(IGenericRepository<FeeDiscountParticularWiseModel, int> feeDiscountParticularRepo,
            IGenericRepository<FeeDiscountModel, int> feeDiscountRepo, IGenericRepository<FeeCategory, int> feeCategoryRepo)
        {
            _feeDiscountParticularRepo = feeDiscountParticularRepo;
            _feeDsicountRepo = feeDiscountRepo;
            _feeCategoryRepo = feeCategoryRepo;
        }
        public async Task<IActionResult> Index(int id)
        {
            if (id != 0)
            {
                FeeDiscountViewModel model = new FeeDiscountViewModel();
                var feeDiscountModel = await _feeDsicountRepo.GetSingle(x => x.IsActive == 1 && x.IsDeleted == 0 && x.Id == id);
                model.FeeDiscountVm = feeDiscountModel;
                var feeDiscountParticualVms = await _feeDiscountParticularRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1 && x.FeeDiscountId == feeDiscountModel.Id);
                model.FeeDiscountParticualVms = feeDiscountParticualVms.ToList();
                return PartialView("~/Views/FeeTransaction/_FeeDiscountPartial.cshtml", model);
            }

            return await Task.Run(() => PartialView("~/Views/FeeTransaction/_FeeDiscountPartial.cshtml"));
        }

        public async Task<JsonResult> GetParticularList(int id)
        {
            if (id == 0)
            {
                var result = await _feeCategoryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                return Json(result);
            }
            else
            {
                var feeDiscount = await _feeDsicountRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0 && x.Id == id);
                var feeDiscountParticularwise = await _feeDiscountParticularRepo.GetList(x => x.IsDeleted == 0 && x.IsActive == 1);
                var feeCategory = await _feeCategoryRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

                var result = (from FD in feeDiscount
                              join FDP in feeDiscountParticularwise
                              on FD.Id equals FDP.FeeDiscountId
                              join FC in feeCategory
                              on FDP.ParticularId equals FC.Id
                              select new FeeDiscountParticularWiseModel
                              {
                                  Id=FDP.Id,
                                  ParticularId = FC.Id,
                                  CategoryName = FC.Name,
                                  DiscountValue = FDP.DiscountValue,
                                  DiscountType = FDP.DiscountType,
                                  Description = FDP.Description,
                              }).ToList();

                return Json(result);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreatePerticularDiscount(FeeDiscountViewModel model)
        {
            var perticularDiscountType = Request.Form["ParticularDiscountType"].ToString().Split(',');
            var particularDiscountValue = Request.Form["PerticularDiscountValue"].ToString().Split(',');
            var particularDescription = Request.Form["PerticularDescription"].ToString().Split(',');
            var categoriesId = Request.Form["CategoryId"];
            var discountPerticularId = Request.Form["discountPertId"];


            if (model.FeeDiscountVm.Id == 0)
                return await CreateFeeDiscount(model, perticularDiscountType, particularDiscountValue, particularDescription, categoriesId);
            else
                return await UpdateFeeDiscount(model, perticularDiscountType, particularDiscountValue, particularDescription, categoriesId, discountPerticularId);
        }
        public async Task<IActionResult> GetDiscontList()
        {
            var result = await _feeDsicountRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/FeeTransaction/_FeeDiscountListPartial.cshtml", result);
        }

        #region Private
        private async Task<IActionResult> CreateFeeDiscount(FeeDiscountViewModel model, string[] perticularDiscountType, string[] particularDiscountValue, string[] particularDescription, StringValues categoriesId)
        {
            var result = await _feeDsicountRepo.Add(model.FeeDiscountVm);

            if (result != ResponseStatus.AddedSuccessfully)
                return Json(ResponseData.Instance.GenericResponse(result));

            if (model.FeeDiscountVm.DependentOnParticular == 1)
            {
                await _feeDsicountRepo.CreateNewContext();
                var feeDiscountList = await _feeDsicountRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
                var maxDiscountId = feeDiscountList.Max(x => x.Id);

                List<FeeDiscountParticularWiseModel> models = new List<FeeDiscountParticularWiseModel>();
                for (int i = 0; i < perticularDiscountType.Count(); i++)
                {
                    var perticulaModel = new FeeDiscountParticularWiseModel()
                    {
                        DiscountType = perticularDiscountType[i].ToString(),
                        DiscountValue = Convert.ToDecimal(particularDiscountValue[i]),
                        Description = particularDescription[i].ToString(),
                        ParticularId = Convert.ToInt32(categoriesId[i]),
                        FeeDiscountId = maxDiscountId
                    };
                    models.Add(perticulaModel);
                }

                var resultCategory = await _feeDiscountParticularRepo.Add(models.ToArray());

                return Json(ResponseData.Instance.GenericResponse(resultCategory));
            }

            return Json(ResponseData.Instance.GenericResponse(ResponseStatus.AddedSuccessfully));
        }

        private async Task<IActionResult> UpdateFeeDiscount(FeeDiscountViewModel model, string[] perticularDiscountType, string[] particularDiscountValue, string[] particularDescription, StringValues categoriesId, string[] discountPerticularId)
        {
            var result = await _feeDsicountRepo.Update(model.FeeDiscountVm);

            if (result != ResponseStatus.UpdatedSuccessFully)
                return Json(ResponseData.Instance.GenericResponse(result));

            if (model.FeeDiscountVm.DependentOnParticular == 1)
            {
                List<FeeDiscountParticularWiseModel> models = new List<FeeDiscountParticularWiseModel>();
                for (int i = 0; i < perticularDiscountType.Count(); i++)
                {
                    var perticulaModel = new FeeDiscountParticularWiseModel()
                    {
                        DiscountType = perticularDiscountType[i].ToString(),
                        DiscountValue = Convert.ToDecimal(particularDiscountValue[i]),
                        Description = particularDescription[i].ToString(),
                        ParticularId = Convert.ToInt32(categoriesId[i]),
                        FeeDiscountId = model.FeeDiscountVm.Id,
                        Id=Convert.ToInt32(discountPerticularId[i])

                    };
                    models.Add(perticulaModel);
                }

                var resultCategory = await _feeDiscountParticularRepo.Update(models.ToArray());

                return Json(ResponseData.Instance.GenericResponse(resultCategory));
            }

            return Json(ResponseData.Instance.GenericResponse(ResponseStatus.AddedSuccessfully));
        }
        #endregion
    }
}