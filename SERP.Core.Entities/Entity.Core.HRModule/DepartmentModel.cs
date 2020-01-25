using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("Department", Schema= "HR")]
    public class DepartmentModel :Base<int>
    {
        [Required(ErrorMessage ="Please enter Department Name")]
        [Display(Name ="Enter Department Name", Prompt ="Enter Department Name")]
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
