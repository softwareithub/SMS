using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class RootObjectModel
    {
        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; } = true;
        public bool selected { get; set; } = true;
    }
}
