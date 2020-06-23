using Microsoft.AspNetCore.Mvc;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Models.ViewComponents
{
    public class AbsentTeacherViewComponent : ViewComponent
    {
        private readonly IDashBoardGraphRepo _IDashBoardRepo;
        public AbsentTeacherViewComponent(IDashBoardGraphRepo dashBoardGraphRepo)
        {
            _IDashBoardRepo = dashBoardGraphRepo;
        }
        public async Task<IViewComponentResult> InvokeAsync(DateTime date)
        {
            date = DateTime.Now.Date;
            var result = (await _IDashBoardRepo.GetAbsentTeacher(date));
            return await Task.Run(() => (IViewComponentResult)View("~/Views/DashBoard/_AbsentTeacherPartial.cshtml", result));
        }
    }
}
