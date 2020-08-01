using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Entities.Entity.Core.Transaction;
using SERP.Infrastructure.Repository.Infrastructure.Repo;

namespace SERP.UI.Controllers.QuickSearch
{
    public class MasterQuickSearchController : Controller
    {
        private readonly IGenericRepository<StudentMaster, int> _studentRepo;
        private readonly IQuickSearchRepo _QuickSearchRepo;

        public MasterQuickSearchController(IGenericRepository<StudentMaster, int> studentRepo, IQuickSearchRepo quickSearchRepo)
        {
            _studentRepo = studentRepo;
            _QuickSearchRepo = quickSearchRepo;
        }
        public async Task<IActionResult> Index(string studentName)
        {
            studentName = studentName.Split("(")[0];
            var studentId = (await _studentRepo.GetSingle(x => x.Name.ToLower().Trim() == studentName.ToLower().Trim())).Id;
            HttpContext.Session.SetInt32("StudentId",studentId);
            return await Task.Run(() => PartialView("~/Views/QuickMasterSearch/_QuickStudentSearchPartial.cshtml"));
        }

        public async Task<IActionResult> StudentBasicInfo()
        {
            ViewBag.HeaderDetail = "Student Basic Information";
            var model =await  _QuickSearchRepo.GetStudentBasicInfo(Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")));
            return PartialView("~/Views/QuickMasterSearch/_StudentBasicInfoPartial.cshtml", model);
        }

        public async Task<IActionResult> GetStudentAttandenceReport(int monthId, int yearId)
        {
            ViewBag.HeaderDetail = "Student Attandence Report";
            var models = await _QuickSearchRepo.GetStudentAttandenceReport(Convert.ToInt32(HttpContext.Session.GetInt32("StudentId")), monthId, yearId);
            return PartialView("~/Views/QuickMasterSearch/_StudentAttandenceReportPartial.cshtml", models);
        }
    }
}