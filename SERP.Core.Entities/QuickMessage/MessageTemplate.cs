using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.QuickMessage
{
    [Table("MessageTemplate", Schema = "Master")]
    public class MessageTemplate:Base<int>
    {
        [Required(ErrorMessage ="Template Type name is required !")]
        [Display(Prompt ="Please enter Template Type name.")]
        public string TemplateTypeName { get; set; }

        [Required(ErrorMessage = "Template name is required !")]
        [Display(Prompt = "Please enter Template name.")]
        public string TemplateName { get; set; }

        [Required(ErrorMessage = "Template is required !")]
        [Display(Prompt = "Please enter Template.")]
        public string Template { get; set; }
    }
}
