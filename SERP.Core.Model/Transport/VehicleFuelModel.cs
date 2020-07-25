using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.Transport
{
    public class VehicleFuelModel
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public DateTime FuelDate { get; set; }
        public string ReciptNumber { get; set; }
        public string RecieptImage { get; set; }
        public string Remark { get; set; }
    }
}
