using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeSalaryAllocation",Schema ="HR")]
    public class EmployeeSalaryModel: Base<int>
    {
        public int EmployeeId { get; set; } = default;
        public int HeadId { get; set; } = default;
        public decimal Amount { get; set; } = default;
    }
}
