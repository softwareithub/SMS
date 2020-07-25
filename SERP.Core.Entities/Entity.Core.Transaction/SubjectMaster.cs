using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("Subject", Schema = "TransactionSch")]
    public class SubjectMaster: Base<int>
    {
        [Required(ErrorMessage ="Please enter Course")]
        [Display(Name ="Course", Prompt ="Select Course")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please enter subject code")]
        [Display(Name = "Subject Code", Prompt = "Enter Subject Code")]
        public string SubjectCode { get; set; }
        [Required(ErrorMessage = "Please enter subject name")]
        [Display(Name = "Subject Name", Prompt = "Enter Subject Name")]
        public string SubjectName { get; set; }
        [Display(Name = "Subject Description", Prompt = "Enter Subject Description")]
        public string SubjectDescription { get; set; } = string.Empty;
        public int IsElective { get; set; } = default;
    }
}
