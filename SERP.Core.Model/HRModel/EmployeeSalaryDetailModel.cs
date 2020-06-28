using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.HRModel
{
    public class EmployeeSalaryDetailModel
    {
        public int Id { get; set; }
        public int HeadId { get; set; }
        public string HeadName { get; set; }
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public int IsDependentOnDay { get; set; }
        public string AdditionDeduction { get; set; }
    }
}
