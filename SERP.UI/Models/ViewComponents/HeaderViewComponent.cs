using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SERP.UI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Models.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public static List<HeaderComponet> headerList = new List<HeaderComponet>();
        public async Task<IViewComponentResult> InvokeAsync(string controllerName, string actionName, string name)
        {

            headerList.Add(new HeaderComponet()
            {
                ControllerName = controllerName,
                ActionName = actionName,
                HeaderName = name
            });
            HttpContext.Session.SetObject("headers",headerList);
            var headers = HttpContext.Session.GetObject<List<HeaderComponet>>("headers");
            return await Task.Run(() => (IViewComponentResult)View("~/Views/Shared/_ContentHeader.cshtml", headers));
        }

        public class HeaderComponet
        {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string HeaderName { get; set; }
        }
    }
}
