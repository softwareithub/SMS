using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("Caste",Schema = "Master")]
    public class CasteMaster:Base<int>
    {
        [Required(ErrorMessage ="Caste name is required")]
        [Display(Name ="Caste Name",Prompt ="Enter Caste name")]
        public string Name { get; set; }
        [Display(Name = "Description", Prompt = "Enter Caste Description")]
        public string Description { get; set; } = string.Empty;
        public int SessionId { get; set; }
    }
}
