using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("ExamUpdate")]
    public class ExamUpdate:Base<int>
    {
        [Required(ErrorMessage ="Event Master name is required")]
       public string EventMasterName { get; set; }
        [Required(ErrorMessage = "Event  name is required")]
        public string EventName { get; set; }
        [Required(ErrorMessage = "Event Description  is required")]
        public string EventDescription { get; set; } 
        public string UploadPDFPath { get; set; } = string.Empty;
        public int SequeneId { get; set; } = default;
    }
}
