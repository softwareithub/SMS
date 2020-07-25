using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Accounts
{
    [Table("Account", Schema = "Accounts")]
    public class AccountDetail : Base<int>
    {
        [Required(ErrorMessage ="Account name is required.")]
        [Display(Prompt ="Please Enter Account Name")]
        public string AccountName { get; set; }
        [Required(ErrorMessage ="Opening amount is required.")]
        [DataType(DataType.Currency)]
        [Display(Prompt ="Please Enter Opening Balance")]
        public decimal OpeningBalance { get; set; }
        [Required(ErrorMessage ="Account start date is required.")]
        [DataType(DataType.Date)]
        [Display(Prompt="Please Enter Account Start Date")]
        public DateTime AccountStartDate { get; set; }
    }
}
