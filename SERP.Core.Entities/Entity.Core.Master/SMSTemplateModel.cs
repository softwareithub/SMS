using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("SMSEmailTemplate", Schema ="Master")]
    public class SMSTemplateModel: Base<int>
    {
        [Required(ErrorMessage ="Please enter the Template Name")]
        [Display(Prompt ="Enter Template Name")]
        [DataType(DataType.Text)]
        public string TemplateName { get; set; }
        [Required(ErrorMessage = "Template is required.")]
        [Display(Prompt = "Enter Template")]
        public string Template { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please select Template Type")]
        public string TemplateType { get; set; }
        public int SessionId { get; set; }
    }
}
