using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("SMSLog",Schema="Master")]
    public class SMSBulk: Base<int>
    {
        public int StudentId { get; set; }
        public int EmployeeId { get; set; }
        public int TemplateId { get; set; }
        public string CustomMessage { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan SentTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; }
    }
}
