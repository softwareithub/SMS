using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace SERP.UI.Helper
{
    public static class CustomSelectList
    {
        public static IHtmlContent SerpSelectList<T>(this IHtmlHelper helper,string className ,IEnumerable<T> items,
            string name,string defaultText, string optionText="Name", int? seletedValue=0)
        {
            defaultText=string.IsNullOrEmpty(defaultText) ? "Select" : defaultText;

            string htmlContent = $"<select class="+className+" name="+name+" id="+name+">";
            htmlContent += $"<option value="+string.Empty+">"+defaultText+"</option>";
            foreach (var data in items)
            {
                Type type = data.GetType();
                PropertyInfo pinfoId = type.GetProperty("Id");
                PropertyInfo pinfoName = type.GetProperty(optionText);
                if (Convert.ToInt32(pinfoId.GetValue(data)) == seletedValue)
                {
                    htmlContent += $"<option selected value=" + pinfoId.GetValue(data) + ">" + pinfoName.GetValue(data) + "</option>";
                }
                else
                {
                    htmlContent += $"<option value=" + pinfoId.GetValue(data) + ">" + pinfoName.GetValue(data) + "</option>";
                }
               
            }
            htmlContent += "</select>";
            return new HtmlString(htmlContent);
        }
    }
}
