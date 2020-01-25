using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("IntituteDetail", Schema = "Master")]
    public class InstituteMaster : Base<int>
    {
        [Required(ErrorMessage = "Institute name is required.")]
           
        [Display(Name = "Institute Name", Prompt = "Enter Institute Name"),
            MaxLength(100, ErrorMessage = "Institute name is too long."),
            MinLength(3, ErrorMessage = "Institute name is too short.")]
        public string Name { get; set; }

        [Display(Name = "Institute Code", Prompt = "Enter Institute Code"),
            MaxLength(20, ErrorMessage = "Institute code is too long."),
            MinLength(0, ErrorMessage = "Institute Code is too short.")]
        public string Code { get; set; }

        [Display(Name = "Address", Prompt = "Enter Institute Address"),
            MaxLength(500, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid Email Address")]
        [MaxLength(30, ErrorMessage = "Email Address is too long.")]
        [Display(Name = "Email", Prompt = "Enter Display Email")]
        public string Email { get; set; }

        [RegularExpression(@"\d{5}([- ]*)\d{6}", ErrorMessage = "Invalid phone number")]
        [MaxLength(20, ErrorMessage = "Phone number is too long.")]
        [Display(Name = "Phone", Prompt = "Enter phone number")]
        public string Phone { get; set; }

        [RegularExpression(@"^[789]\d{9}$", ErrorMessage = "Invalid mobile number")]
        [MaxLength(20, ErrorMessage = "Mobile is too long.")]
        [Display(Name ="Mobile number",Prompt ="Enter mobile number.")]
        public string Mobile { get; set; }

        [RegularExpression(@"\d{5}([- ]*)\d{6}", ErrorMessage = "Invalid fax number")]
        [MaxLength(20, ErrorMessage = "Fax number is too long.")]
        [Display(Name ="Fax",Prompt ="Enter Fax Number")]
        public string Fax { get; set; }
        [MaxLength(70,ErrorMessage ="Admin contact number is too long.")]
        [Display(Name ="Admin Contact Person",Prompt ="Enter Admin Information.")]
        public string AdminContactPerson { get; set; }
        public string InstituteLogo { get; set; }
        [MaxLength(100, ErrorMessage = "Rythum is too long.")]
        public string Rythum { get; set; }
    }
}
