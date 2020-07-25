using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text;

namespace SERP.Core.Entities.Transport
{
    [Table("FuelDetail", Schema = "Transport")]
    public class VehicleFuelDetail:Base<int>
    {
        public int VehicleId { get; set; }
        [Required(ErrorMessage ="Quantity is reqquired.")]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage ="Per Unit Rate is required.")]
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }
        [Required(ErrorMessage ="Fuel date is required.")]
        [DataType(DataType.Date)]
        public DateTime FuelDate { get; set; }

        public string ReciptNumber { get; set; }
        public string RecieptImage { get; set; }
        public string Remark { get; set; }
    }
}
