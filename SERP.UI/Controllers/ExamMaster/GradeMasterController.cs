using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.ExamMaster
{
    public class GradeMasterController : Controller
    {
        private readonly IGenericRepository<GradeMaster, int> _IGradeMasterRepo;

        public GradeMasterController(IGenericRepository<GradeMaster, int> _gradeMasterRepo)
        {
            _IGradeMasterRepo = _gradeMasterRepo;
        }
        public async Task<IActionResult> GradeList()
        {
            var result = await _IGradeMasterRepo.GetList(x => x.IsActive == 1);
            return PartialView("~/Views/ExamMaster/_GradeMasterPartial.cshtml",result);
        }
    }
}