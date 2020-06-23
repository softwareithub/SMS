using Microsoft.AspNetCore.Mvc;
using SERP.Core.Model.UserManagement;
using SERP.UI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Models.ViewComponents
{
    public class SideMenuViewComponent :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string controllerName, string actionName)
        {
            List<MenuSubMenuVm> menuModels = HttpContext.Session.GetObject<List<MenuSubMenuVm>>("menuSubMenu");
            return await Task.Run(() => (IViewComponentResult)View("~/Views/Shared/MenuSubMenu/_SideMenuPartial.cshtml", menuModels));
        }

    }
}
