﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Model.MasterViewModel;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.Utilities.ResponseMessage;
using SERP.Utilities.ResponseUtilities;

namespace SERP.UI.Controllers.Master
{
    public class BatchMasterController : Controller
    {
        private readonly IGenericRepository<CourseMaster, int> _IClassRepo;
        private readonly IGenericRepository<BatchMaster, int> _IBatchRepo;

        public BatchMasterController(IGenericRepository<CourseMaster, int> classRepo,
                            IGenericRepository<BatchMaster, int> batchRepo)
        {
            _IClassRepo = classRepo;
            _IBatchRepo = batchRepo;
        }
        public async Task<IActionResult> Index(int id = 0)
        {
            await PopulateDefault();
            if (id == 0)
                return PartialView("~/Views/BatchMaster/_CreateBatchPartial.cshtml");

            return PartialView("~/Views/BatchMaster/_CreateBatchPartial.cshtml", await GetBatchMasterById(id));
        }
        [HttpPost]
        public async Task<IActionResult> CreateBatch(BatchMaster model)
        {
            var result=await UpSertBatchMaster(model);
            return Json(result);
        }
        public async Task<IActionResult> GetBatchMasterList()
        {
            var batchResult = await _IBatchRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            await _IClassRepo.CreateNewContext();
            var courseResult = await _IClassRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);

            var batchMasterList = GetBatchMasterList(batchResult, courseResult);

            return PartialView("~/Views/BatchMaster/_BatchListPartial.cshtml", batchMasterList);
        }
        public async Task<IActionResult> DeleteBatch(int Id)
        {
            var batchData = await _IBatchRepo.GetSingle(x => x.Id == Id);
            batchData.IsActive = 0;
            batchData.IsDeleted = 1;
            batchData.UpdatedBy = 1;
            batchData.UpdatedDate = DateTime.Now.Date;

            await _IBatchRepo.CreateNewContext();

            var result = await _IBatchRepo.Delete(batchData);

            return Json("Batch deleted successfully.");
        }
        public async Task<BatchMaster> GetBatchMasterById(int Id)
        {
            var result = await _IBatchRepo.GetSingle(x => x.Id == Id);
            return result;
        }

        #region Private
        public async Task PopulateDefault()
        {
            ViewBag.ClassList = await _IClassRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
        }
        private static IEnumerable<BatchMasterListVm> GetBatchMasterList(IList<BatchMaster> batchResult, IList<CourseMaster> courseResult)
        {
            return batchResult.Join(courseResult, batch => batch.CourseId,
                   courseData => courseData.Id,
                   (batch, courseData) => new BatchMasterListVm
                   {
                       BatchId = batch.Id,
                       CourseCode = courseData.Code,
                       CourseName = courseData.Name,
                       BatchName = batch.BatchName,
                       BatchCode= batch.BatchCode
                   });
        }
        private async Task<string> UpSertBatchMaster(BatchMaster model)
        {
            model.IsActive = 1;
            model.IsDeleted = 0;
            if (model.Id == 0)
            {
                model.CreatedBy = 1;
                model.CreatedDate = DateTime.Now.Date;
                var result = await _IBatchRepo.CreateEntity(model);
                return (ResponseData.Instance.GenericResponse(result));
               
            }
            else
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _IBatchRepo.Update(model);
                return (ResponseData.Instance.GenericResponse(result));
            }
        }
        #endregion
    }
}