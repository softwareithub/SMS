using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Transport
{
    [Table("RouteStopage", Schema = "Transport")]
    public class RouteStopageModel:Base<int>
    {
        [Required(ErrorMessage ="Route is required.")]
        public int RouteId { get; set; }
        [Required(ErrorMessage ="Stopage is required.")]
        public int StopageId { get; set; }
        public string RouteType { get; set; }

    }
}
