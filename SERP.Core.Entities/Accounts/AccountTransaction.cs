using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Accounts
{
    [Table("AccountTransaction", Schema = "Accounts")]
    public class AccountTransaction: Base<int>
    {
		[Required(ErrorMessage ="Please select Account")]
		public int AccountId { get; set; }
		[Required(ErrorMessage = "Please select Expense")]
		public int ExpenseId { get; set; }
		[Required(ErrorMessage = "Please select Income")]
		public int IncomeId { get; set; }
		[Required(ErrorMessage = "Please Enter Amount")]
		[DataType(DataType.Currency)]
		[Display(Prompt ="Enter Amount")]
		public decimal Amount { get; set; }
		[Required(ErrorMessage ="Date of Expense is required")]
		[DataType(DataType.Date)]
		[Display(Prompt ="Date of Expense")]
		public DateTime DateOfIncomeExpense { get; set; }
		public int StudentId { get; set; } = default;
		public string Purpose { get; set; } = string.Empty;
		public string Remarks { get; set; } = string.Empty;
		public string RecieptNumber { get; set; } = string.Empty;
		public int SessionId { get; set; }

	}
}
