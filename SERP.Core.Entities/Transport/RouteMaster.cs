using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Transport
{
    [Table("RouteMaster",Schema = "Transport")]
    public class RouteMaster: Base<int>
    {
        [Required(ErrorMessage ="Route name is required.")]
        public string RouteName { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int SessionId { get; set; }
    }
}
