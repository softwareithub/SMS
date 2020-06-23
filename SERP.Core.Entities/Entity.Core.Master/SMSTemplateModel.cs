using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("SMSTemplate",Schema ="Master")]
    public class SMSTemplateModel: Base<int>
    {
        public string TemplateName { get; set; }
        public string Template { get; set; }
        public int IsSchedule { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan ScheduleTime { get; set; }
    }
}
