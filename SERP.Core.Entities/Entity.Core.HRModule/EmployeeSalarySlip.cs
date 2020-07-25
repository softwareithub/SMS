using SERP.Core.Entities.Entity.Core.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    public class EmployeeSalarySlip
    {
        public List<Tuple<string, decimal>> PaySalary { get; set; }
        public InstituteMaster InstituteMaster { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string FatherName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string HeadName { get; set; }
        public int IsDependentPerDay { get; set; }
        public string AdditionDeduction { get; set; }
        public decimal Amount { get; set; }
        public decimal PerDayAmount { get; set; }
        public int AbsentDays { get; set; }
        public int PresentDays { get; set; }

        public decimal PayAmount { get; set; }
    }
}
