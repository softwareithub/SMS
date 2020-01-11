using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{

    public static class CustomTagHelper 
    {
        public static IHtmlContent SerpBoldLabel(this IHtmlHelper htmlHelper,string value)
            => new HtmlString($"<strong>{value}</strong>");
     
    }

}
