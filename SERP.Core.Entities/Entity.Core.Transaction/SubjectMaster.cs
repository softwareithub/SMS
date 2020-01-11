using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("Subject",Schema = "TransactionLog")]
    public class SubjectMaster: Base<int>
    {
        
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public int IsElective { get; set; }
    }
}
