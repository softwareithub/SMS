using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;


namespace SERP.UI.Helper
{
    public static class ImageHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static IHtmlContent SerpImageHelper(this IHtmlHelper htmlHelper, string imgsrc, int height, int width, string Id)
        {
            if(!string.IsNullOrEmpty(imgsrc))
            {
                imgsrc = imgsrc.Replace("~", string.Empty);
            }
            string basepath="http://"+SERPHttpContextAccessor.Current.Request.Host.ToString();
            var imagePath = basepath+ "//Images/UserLogo.jpg";
            imgsrc = string.IsNullOrEmpty(imgsrc) ? imagePath : basepath+"//"+imgsrc;
            string htmlContent = $"<img id={Id} src={imgsrc} style=width:{width}px;height:{height}px>";
            return new HtmlString(htmlContent);
        }
    }
}
