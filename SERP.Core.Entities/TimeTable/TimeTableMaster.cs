using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.TimeTable
{
    [Table("TimeTable", Schema = "Master")]
    public class TimeTableMaster:Base<int>
    {
		[Required(ErrorMessage ="Please select Course")]
		[Display(Prompt ="Select Course")]
		public int CourseId { get; set; }

		[Required(ErrorMessage = "Please select Batch")]
		[Display(Prompt = "Select Batch")]
		public int BatchId { get; set; }

		[Required(ErrorMessage = "Please select Subject")]
		[Display(Prompt = "Select Subject")]
		public int SubjectId { get; set; }

		[Required(ErrorMessage = "Please select Teacher")]
		[Display(Prompt = "Select Teacher")]
		public int EmployeeId { get; set; }
		public int IsClassTeacher { get; set; }
		public int IsAttendClass { get; set; }

		[DataType(DataType.Time)]
		[Required(ErrorMessage ="From Time is required.")]
		public TimeSpan FromTime { get; set; }
		[DataType(DataType.Time)]
		[Required(ErrorMessage = "To Time is required.")]
		public TimeSpan ToTime { get; set; }
		public DateTime TimeTableDate { get; set; }
		public int SessionId { get; set; }

		[NotMapped]
		[Required(ErrorMessage ="Please select Days")]
		public string[] Days { get; set; }
	}
}
