using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace SERP.Core.Entities.Transport
{
    [Table("RouteStudentMapping", Schema = "Transport")]
    public class RouteStudentMapping:Base<int>
    {
        public int RouteId { get; set; }
        public int StopageId { get; set; }
        public int StudentId { get; set; }
        public int SessionId { get; set; }
    }
}
