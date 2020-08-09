using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.SERPExceptionLogging
{
    [Table("ExceptionLogging", Schema = "Master")]
    public class ExceptionLogging: Base<int>
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionNature { get; set; }
        public int IsResolved { get; set; } = 0;
    }
}
