using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SERP.Core.Entities.Accounts
{
	[Table("IncomeHeads", Schema = "Accounts")]
    public class IncomeHeads:Base<int>
    {
		[Required(ErrorMessage ="Head name is required.")]
		[DataType(DataType.Text)]
		[Display(Prompt ="Please Enter Income Name")]
		
		public string Name { get; set; }

		[Required(ErrorMessage ="Income head code is required.")]
		[Display(Prompt ="Please Enter Income Code")]
		public string IncomeCode { get; set; }
		public int SessionId { get; set; }
	}
}
