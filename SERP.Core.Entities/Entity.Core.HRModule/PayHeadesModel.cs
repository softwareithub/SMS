using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("PayHeads",Schema ="HR")]
    public class PayHeadesModel :Base<int>
    {
        [Required(ErrorMessage ="Enter Pay Head Name")]
        [Display(Name="PayHead",Prompt ="Enter PayHead Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Pay Head Code")]
        [Display(Name = "PayCode", Prompt = "Enter PayHead Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Enter Pay Head Dependent")]
        [Display(Name = "IsDependent", Prompt = "Enter PayHead Dependent")]
        public int IsDependentPerDay { get; set; }
        [Required(ErrorMessage = "Enter Pay Head Deuction Addition")]
        [Display(Name = "AdditionDeduction", Prompt = "Enter Deuction Addition")]
        public string Addition_Deduction { get; set; }
    }
}
