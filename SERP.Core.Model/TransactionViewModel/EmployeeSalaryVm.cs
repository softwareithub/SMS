using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class EmployeeSalaryVm
    {
        public int EmployeeId { get; set; }
        public int SalaryHead { get; set; }
        public decimal Amount { get; set; }
        public int EmployeeSalaryId { get; set; }
        public string HeadName { get; set; }
        public string AdditionDeduction { get; set; }
    }
}
