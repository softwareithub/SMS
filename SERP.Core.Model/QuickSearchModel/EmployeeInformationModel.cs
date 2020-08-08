using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SERP.Core.Model.QuickSearchModel
{
    public class EmployeeInformationModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CAddress { get; set; }
        public string P_Address { get; set; }
        public string Photo { get; set; }
        public string EmergencyPhone { get; set; }
        public int ConvictedToCrime { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string AccountNumber { get; set; }
        public List<EmployeeSalaryHead> EmployeeSalaryHeads { get; set; }
        public List<EmployeeAttandence> EmployeeAttandence { get; set; }
    }

    public class EmployeeSalaryHead
    {
        public string HeadName { get; set; }
        public string AdditionDeduction { get; set; }
        public decimal Amount { get; set; }
        public int IsDependentOnPerDay { get; set; }
    }

    public class EmployeeAttandence
    {
        public int AttendenceCount { get; set; }
        public string AttendenceType { get; set; }
        public int DateMonth { get; set; }
        public string DateMonthName { get; set; }
        public int DateYear { get; set; }
    }

}
