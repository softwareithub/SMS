using SERP.Core.Entities.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.Transport
{
    public class RouteViewModel
    {
        public RouteMaster RouteMaster { get; set; }
        public List<RouteStopageModel> RouteStopageModels { get; set; }
    }
}
