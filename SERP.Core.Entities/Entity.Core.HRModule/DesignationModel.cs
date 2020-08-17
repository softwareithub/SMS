using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("Designation",Schema= "HR")]
    public class DesignationModel:Base<int>
    {
        [Required(ErrorMessage ="Please enter designation name")]
        [Display(Name="Designation Name",Prompt ="Enter Designation Name")]
        public string Name { get; set; }
        [Display(Name = "Designation Code", Prompt = "Enter Designation Code")]
        public string Code { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
