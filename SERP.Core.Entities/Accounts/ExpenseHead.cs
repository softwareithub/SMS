using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Accounts
{
    [Table("ExpenseHeads",Schema = "Accounts")]
    public class ExpenseHead:Base<int>
    {
        [Required(ErrorMessage ="Expense head name is required.")]
        [DataType(DataType.Text)]
        [Display(Prompt ="Please Enter Expense Head Name")]
		public string Name { get; set; }
        [Required(ErrorMessage ="Expense Head Code is required.")]
        [Display(Prompt ="Please Enter Expense Head Code.")]
		public string HeadCode { get; set; }

	}
}
