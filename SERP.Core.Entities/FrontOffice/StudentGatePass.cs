using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.FrontOffice
{
    [Table("StudentGatePass", Schema = "Master")]
   public class StudentGatePass:Base<int>
    {
		[Required(ErrorMessage ="Please select StudentId")]
		[Display(Prompt ="Select Student")]
		public int StudentId { get; set; }
		[Required(ErrorMessage ="Please select Approved By")]
		public string ApproveBy { get; set; }
		
		public int GuradianId { get; set; }
		[Required(ErrorMessage = "Please enter reason to leave early")]
		[Display(Prompt ="Enter Reason To Early Leave")]
		public string Reason { get; set; }
		[DataType(DataType.DateTime)]
		[Required(ErrorMessage = "Please select Student Leave")]
		public DateTime StudentLeave { get; set; }
		[NotMapped]
		[Required(ErrorMessage ="Guardian Name is required.")]
		public string GuardianName { get; set; }
	}
}
