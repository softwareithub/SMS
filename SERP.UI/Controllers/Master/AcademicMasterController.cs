using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using SERP.Utilities.ResponseMessage;
using System.Threading;

namespace SERP.UI.Controllers.Master
{
    public class AcademicMasterController : Controller
    {
        private readonly IGenericRepository<AcademicMaster, int> _IGenericRepo;
        public AcademicMasterController(IGenericRepository<AcademicMaster, int>  genericRepository)
        {
            _IGenericRepo = genericRepository;
        }
        public async Task<IActionResult> Index(int id)
        {
           
            if (id==0)
                return await Task.Run(() => PartialView("~/Views/AcademicMaster/Index.cshtml"));

            var result =await  _IGenericRepo.GetSingle(x => x.Id == id);
            return PartialView("~/Views/AcademicMaster/Index.cshtml", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(AcademicMaster model)
        {
            if (model.Id == 0)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = 1;
                var result = await _IGenericRepo.CreateEntity(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
            else
            {
                model.UpdatedBy = 1;
                model.UpdatedDate = DateTime.Now.Date;
                var result = await _IGenericRepo.Update(model);
                return Json(ResponseData.Instance.GenericResponse(result));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAcademicList()
        {
            var result = await _IGenericRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/AcademicMaster/_AcademicList.cshtml",result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRecord(int id)
        {

            Thread.Sleep(3000);
            var academicModel = await _IGenericRepo.GetSingle(x => x.Id == id);
            academicModel.IsActive = 0;
            academicModel.IsDeleted = 0;

            await _IGenericRepo.CreateNewContext();

            var result = await _IGenericRepo.Delete(academicModel);
            return Json(ResponseData.Instance.GenericResponse(result));
        }
    }
}