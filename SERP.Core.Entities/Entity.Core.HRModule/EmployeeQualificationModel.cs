using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeQualification", Schema="HR")]
    public class EmployeeQualificationModel
    {
        public int EmployeeId { get; set; } = default;
        public string DegreeName { get; set; } = string.Empty;
        public string InstituteName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Years { get; set; } = string.Empty;
    }
}
