using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeQualification", Schema="HR")]
    public class EmployeeQualificationModel
    {
        public int EmployeeId { get; set; }
        public string DegreeName { get; set; }
        public string InstituteName { get; set; }
        public string Location { get; set; }
        public string Years { get; set; }
    }
}
