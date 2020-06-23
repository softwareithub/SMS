using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.ExamDetail
{
    [Table("GradeMaster", Schema = "Master")]
    public class GradeMaster:Base<int>
    {
        public string Grade { get; set; }
        public string Percentage { get; set; }
    }
}
