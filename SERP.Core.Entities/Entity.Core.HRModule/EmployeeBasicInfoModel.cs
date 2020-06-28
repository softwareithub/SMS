using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("EmployeeBasicInfo",Schema ="HR")]
    public class EmployeeBasicInfoModel:Base<int>
    {
        [Required(ErrorMessage ="Employee Code is required.")]
        public string EmpCode { get; set; }
        [Required(ErrorMessage = "Employee name is required.")]
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Phone is required")]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string C_Address { get; set; }
        public string p_Address { get; set; }
        public string Photo { get; set; }
        public string EmergencyPhone { get; set; }
        public int? ConvictedForCrime { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; }
    }
}
