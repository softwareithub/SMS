using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
	[Table("StudentDocumentUpload", Schema = "Master")]
    public class StudentDocumentUpload:Base<int>
    {
		[Required(ErrorMessage ="Please select Student")]
		public int StudentId { get; set; }
		[Required(ErrorMessage = "Please Enter Document Type")]
		[Display(Prompt ="Enter Document Type")]
		public string DocumentName { get; set; }
		[Required(ErrorMessage = "Please Enter Document Path")]
		[Display(Prompt = "Enter Document Path")]
		public string DocumentPath { get; set; }
		[Required(ErrorMessage = "Please Enter Document Description")]
		[Display(Prompt = "Enter Document Description")]
		public string DocumentDescription { get; set; }
		[NotMapped]
		public string StudentName { get; set; }

	}
}
