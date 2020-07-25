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
        [Required(ErrorMessage ="Father name is required.")]
        public string FatherName { get; set; }
        public string MotherName { get; set; } = string.Empty;
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Phone is required")]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required.")]
        public string C_Address { get; set; } = string.Empty;
        public string p_Address { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone is required.")]
        public string EmergencyPhone { get; set; } = string.Empty;
        public int? ConvictedForCrime { get; set; }
        [Required(ErrorMessage ="Gender is required.")]
        public string Gender { get; set; }
        public string BloodGroup { get; set; } = string.Empty;
        public int? DepartmentId { get; set; } = default;
        public int? DesignationId { get; set; } = default;
        [Required(ErrorMessage ="Joining date is required.")]
        public DateTime? JoiningDate { get; set; }
        public int? BankId { get; set; } = default;
        public string AccountNumber { get; set; } = string.Empty;
    }
}
