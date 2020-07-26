using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
	[Table("StudentEducationalDetail", Schema ="Master")]
    public class StudentEducationalDetail:Base<int>
    {
		[Required(ErrorMessage ="Please select student")]
		[Display(Prompt ="Select Student")]
		public int StudentId { get; set; }

		[Required(ErrorMessage = "Please Enter University")]
		[Display(Prompt = "Please Enter University")]
		public string University { get; set; }

		[Required(ErrorMessage = "Please Enter College Name")]
		[Display(Prompt = "Please Enter College Name")]
		public string CollegeName { get; set; }

		[Required(ErrorMessage = "Please Enter Course Name")]
		[Display(Prompt = "Please Enter Course Name")]
		public string CourseName { get; set; }

		[Required(ErrorMessage = "Please Enter Joining Year")]
		[Display(Prompt = "Please Enter Joining Year")]
		public int YearOfJoining { get; set; }

		[Required(ErrorMessage = "Please Enter Passing Year")]
		[Display(Prompt = "Please Enter Passing Year")]
		public int YearOfPassing { get; set; }

		[Required(ErrorMessage = "Please Enter Enrollement")]
		[Display(Prompt = "Please Enter Enrollement")]
		public string EnrollmentNumber { get; set; }

		[NotMapped]
		public string StudentName { get; set; }

	}
}
