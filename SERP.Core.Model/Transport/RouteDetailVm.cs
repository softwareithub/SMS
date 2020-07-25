using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.Transport
{
    public class RouteDetailVm
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string DriverName { get; set; }
        public string  VehicleName { get; set; }
        public string VehicleType { get; set; }
    }

    public class StopageDetails
    {
        public int StopageId { get; set; }
        public string StopageName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string RouteType { get; set; }
    }
}
