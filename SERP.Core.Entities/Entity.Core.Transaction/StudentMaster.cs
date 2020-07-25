using SERP.Core.Entities.Validator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("Student", Schema = "TransactionSch")]
    public class StudentMaster : Base<int>
    {
        [Display(Name = "Academic Name", Prompt = "Enter Academic name")]
        [Required(ErrorMessage = "Please select Academic Year")]
        public int AcademicId { get; set; }
        [Required(ErrorMessage ="Registration number is required.")]
        [Display(Name = "Registration Number", Prompt = "Enter Registration number")]
        [MaxLength(110, ErrorMessage = "Registration number is too long.")]
        public string RegistrationNumber { get; set; } = string.Empty;
        [Display(Name = "Joining Date", Prompt = "Enter Joining Date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter Date of Birth")]
        [Required(ErrorMessage = "Joining Date is required")]
        public DateTime JoiningDate { get; set; }
        [Required(ErrorMessage = "Please select Course")]
        public int CourseId { get; set; } = default;
        public int BatchId { get; set; }
        [Display(Name = "Roll Code", Prompt = "Enter Roll Code")]
        [Required(ErrorMessage = "Roll Code is required")]
        [MaxLength(50,ErrorMessage ="Roll code is too long.")]
        public string RollCode { get; set; }
        [Display(Name = "Student Name", Prompt = "Enter Student name")]
        [Required(ErrorMessage = "Student name is required.")]
        [MaxLength(150,ErrorMessage ="Name is too long.")]
        public string Name { get; set; }
        [Display(Name = "Father Name", Prompt = "Enter Father name")]
        [Required(ErrorMessage = "Father name is required")]
        [MaxLength(150, ErrorMessage = "Father name is too long.")]
        public string FatherName { get; set; }
        [Display(Name = "Mother Name", Prompt = "Enter Mother name")]
        [Required(ErrorMessage = "Mother name is required.")]
        [MaxLength(150, ErrorMessage = "Mother name is too long.")]
        public string MotherName { get; set; }
        [Display(Name = "Date of Birth", Prompt = "Enter Date Birth")]
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [CustomDateValidator]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public int? Religion { get; set; } = 0;
        public int? FeeCategoryId { get; set; } = 0;

        [Display(Name = "Father Email", Prompt = "Enter Father Email")]
        [DataType(DataType.EmailAddress)]
        public string FatherEmail { get; set; } = string.Empty;
        [Display(Name = "Student Email", Prompt = "Enter Student Email")]

        [DataType(DataType.EmailAddress)]
        public string StudentEmail { get; set; } = string.Empty;
        [Display(Name = "Father Phone", Prompt = "Enter Father Phone")]
        [Required(ErrorMessage = "Father phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[6-9]\\d{9}",ErrorMessage ="Invalid phone number")]
        public string FatherPhone { get; set; }
        [Display(Name = "Emergency Phone", Prompt = "Enter Emergency Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[6-9]\\d{9}", ErrorMessage = "Invalid phone number")]
        public string EmergencyPhone { get; set; } = string.Empty;
        [Display(Name = "Student Phone", Prompt = "Enter Student Phone")]
        [Required(ErrorMessage = "Student phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[6-9]\\d{9}", ErrorMessage = "Invalid phone number")]
        public string StudentPhone { get; set; }
        [Display(Name = "Mother Phone", Prompt = "Enter Mother Phone")]
        [RegularExpression("^[6-9]\\d{9}", ErrorMessage = "Invalid phone number")]
        public string MotherPhone { get; set; } = string.Empty;
        [Display(Name = "Permanet Address", Prompt = "Enter Permanent Address")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "Permanent address is too long.")]
        public string P_Address { get; set; } = string.Empty;
        [Display(Name = "Crosspondence Address", Prompt = "Enter Crosspondence Address")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250, ErrorMessage = "Crosspondence address is too long.")]
        public string C_Address { get; set; } = string.Empty;
        public string StudentPhoto { get; set; } = string.Empty;
        public string ParentsPhoto { get; set; } = string.Empty;
    }
}
