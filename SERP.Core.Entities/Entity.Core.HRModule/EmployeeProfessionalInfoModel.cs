using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeProfessionalInfo", Schema ="HR")]
    public class EmployeeProfessionalInfoModel: Base<int>
    {
        public int EmployeeId { get; set; } = default;
        public int DepartmentId { get; set; } = default;
        public int DesignationId { get; set; } = default;
        public int SessionId { get; set; }
    }
}
