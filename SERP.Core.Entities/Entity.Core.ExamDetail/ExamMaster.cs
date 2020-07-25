using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("ExamMaster",Schema = "Exam")]
    public class Exam :Base<int>
    {
        [Required(ErrorMessage ="Exam Name is required")]
        [Display(Name ="Exam Name" ,Prompt ="Enter Display Exam Name")]
        public string ExamName { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
    }
}
