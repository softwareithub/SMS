using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;

namespace SERP.UI.Controllers.Master
{
    public class AcademicMasterController : Controller
    {
        private readonly IGenericRepository<AcademicMaster, int> _IGenericRepo;

        public AcademicMasterController(IGenericRepository<AcademicMaster, int>  genericRepository)
        {
            _IGenericRepo = genericRepository;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => PartialView());
        }
        [HttpPost]
        public async Task<IActionResult> Create(AcademicMaster model)
        {
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = 1;
            model.IsActive = 1;
            model.IsDeleted = 0;
            var result = await _IGenericRepo.CreateEntity(model);
            return await Task.Run(() => Json("Data Added"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAcademicList()
        {
            var result = await _IGenericRepo.GetList(x => x.IsActive == 1 && x.IsDeleted == 0);
            return PartialView("~/Views/AcademicMaster/_AcademicList.cshtml",result);
        }
    }
}