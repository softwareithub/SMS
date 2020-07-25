using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{
    public static  class CustomButtonHelper
    {
        public  static IHtmlContent SERPButton(this IHtmlHelper htmlHelper, string className, int insertUpdate)
        {
            string text = insertUpdate == 0 ? "Submit" : "Update";
            string htmlContent = string.Empty;
            if (text== "Submit")
            {
                 htmlContent = $" <button type='submit' class='{className}'><span class='glyphicon glyphicon-save'></span>{text}</button>";
            }
            else
            {
                htmlContent = $" <button type='submit' class='{className}'><span class='glyphicon glyphicon-edit'></span>{text}</button>";
            }
         
            return new HtmlString(htmlContent);
        }
    }
}
