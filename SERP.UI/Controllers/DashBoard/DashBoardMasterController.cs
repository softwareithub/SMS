using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SERP.Core.Model.DashBoardModel;
using SERP.Core.Model.UserManagement;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using SERP.UI.Extension;

namespace SERP.UI.Controllers.DashBoard
{
    public class DashBoardMasterController : Controller
    {
        private readonly IDashBoardGraphRepo _IDashBoardRepo;

        public DashBoardMasterController(IDashBoardGraphRepo dashBoardRepo)
        {
            _IDashBoardRepo = dashBoardRepo;
        }
        public async Task<IActionResult> GetStudentCourseBatchStrenght()
        {
          
            var result = await _IDashBoardRepo.BatchCourseCount();
            return Json(result);
        }

        public async Task<IActionResult> StudentAttendeceReport()
        {
            var result = await _IDashBoardRepo.GetRootList<DateTime>(DateTime.Now.Date);
            return await  Task.Run(() => Json(result));
        }

        public async Task<IActionResult> StudentAttendeceReportMonthDayWise(int year, int month, int day)
        {
            year = year == 0 ? DateTime.Now.Year : year;
            month = month == 0 ? DateTime.Now.Month : month;
            day = day == 0 ? DateTime.Now.Day : day;
            var result = await _IDashBoardRepo.GetStudentAttendenceReport(year, month, day);
            return await Task.Run(() => Json(result));
        }

        public async Task<IActionResult> MenuSubMenuDetails()
        {
            var menuSubMenuDetails = HttpContext.Session.GetObject<List<MenuSubMenuVm>>("menuSubMenu");
            return await Task.Run(()=> PartialView("~/Views/DashBoard/_MenuSubMenuDetails.cshtml", menuSubMenuDetails));
        }
        public async Task<IActionResult> GetMenuDescription()
        {
            return await Task.Run(() => PartialView("~/Views/Shared/_SubMenuDescriptionPartial.cshtml"));
        }
    }
}