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
        public int StudentId { get; set; } = default;
        public int EmployeeId { get; set; } = default;
        public int TemplateId { get; set; } = default;
        public string CustomMessage { get; set; } = string.Empty;
        [DataType(DataType.Time)]
        public TimeSpan SentTime { get; set; } = default;
        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; } = default;
    }
}
