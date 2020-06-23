using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SERP.UI.Models.ViewComponents
{
    public class NewPortalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var url = "https://newsapi.org/v2/top-headlines?" + "country=in&" + "apiKey=151490ca09f645a4b25f580af42584e9";
            var json = new WebClient().DownloadString(url);
            var data=JsonConvert.DeserializeObject<RootObject>(json);
            return await Task.Run(() => (IViewComponentResult)View("~/Views/DashBoard/_NewsPortal.cshtml", data));
        }
    }
}
