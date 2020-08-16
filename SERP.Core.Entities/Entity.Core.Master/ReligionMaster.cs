using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("Religion", Schema = "Master")]
    public class ReligionMaster: Base<int>
    {
        [Display(Name = "Religion Name", Prompt = "Enter religion name")]
        [Required(ErrorMessage ="Please enter religion name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Religion Description", Prompt = "Enter description name")]
        public string Description { get; set; } = string.Empty;
    }
}
