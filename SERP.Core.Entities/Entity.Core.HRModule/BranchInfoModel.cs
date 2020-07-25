using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("BankDetail", Schema ="HR")]
    public class BranchInfoModel:Base<int>
    {
        [Required(ErrorMessage ="Please Enter Branch Name.")]
        [Display(Name="Branch Name", Prompt ="Enter Branch Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Branch Address.")]
        [Display(Name = "Branch Address", Prompt = "Enter Branch Address")]
        public string BranchAddress { get; set; }

        [Required(ErrorMessage = "Please Enter Branch IFSC Code.")]
        [Display(Name = "IFSC Code", Prompt = "Enter Branch IFSC Code")]
        public string IFSCCode { get; set; }
        [Display(Name = "Branch Code", Prompt = "Enter Branch  Code")]
        public string BranchCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Enter Branch Manager Name.")]
        [Display(Name = "Branch Manager Name", Prompt = "Enter Branch Manager Name")]
        public string BranchManagerName { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Phone.")]
        [Display(Name = "Branch Phone", Prompt = "Enter Branch Phone")]
        public string Phone { get; set; }

        [Display(Name = "Branch Email", Prompt = "Enter Branch Email")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter Contact Person Phone.")]
        [Display(Name = "Branch Manager Name", Prompt = "Enter Branch Manager Phone")]
        public string ContactPersonPhone { get; set; }
        [Display(Name = "Branch Email", Prompt = "Enter Branch Email")]
        public string BranchEmail { get; set; } = string.Empty;
        public string BranchLogo { get; set; } = string.Empty;
    }
}
