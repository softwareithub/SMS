using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("CourseMaster", Schema = "Master")]
    public class CourseMaster : Base<int>
    {
        [Required(ErrorMessage = "Course name is required.")]
        [Display(Name="Course Name",Prompt ="Enter Course name")]
        public string Name { get; set; }
        [Display(Name ="Course code",Prompt ="Enter Course code")]
        [Required(ErrorMessage = "Course code is required.")]
        public string Code { get; set; }
        [Display(Name = "Description", Prompt = "Enter Course Description")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Attendecne Type is required")]
        public string AttendenceType { get; set; }
        [Required(ErrorMessage = "Minimum attendence is required")]
        [Display(Name = "Minimum Attendece Percentage", Prompt = "Enter Minimum Attendence Percentage")]
        public decimal MinimumAttendencePercentage { get; set; }
        [NotMapped]
        public bool IsMapped { get; set; }


    }
}
