using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERP.UI.Helper
{
    [HtmlTargetElement("date-tag-helper")]
    public class CustomDateHelper:TagHelper
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "SERPDateTagHelper";
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();
            sb.AppendFormat("<span>{0}</span>", this.DateTime.ToShortDateString());

            output.PreContent.SetHtmlContent(sb.ToString());
        }
    }
}
