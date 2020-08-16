using Microsoft.AspNetCore.Mvc;
using SERP.Infrastructure.Repository.Infrastructure.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Models.ViewComponents
{
    public class DashBoardHeaderViewComponent: ViewComponent
    {
        private readonly IDashBoardGraphRepo _IDashBoardRepo;
        public DashBoardHeaderViewComponent(IDashBoardGraphRepo dashBoardGraphRepo)
        {
            _IDashBoardRepo = dashBoardGraphRepo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = (await _IDashBoardRepo.GetDashBoardDetails());
            return await Task.Run(() => (IViewComponentResult)View("~/Views/DashBoard/_DashBoardInfoHeaderPartial.cshtml", result));
        }
    }
}
