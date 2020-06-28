using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.HRModel
{
    public class LeaveAllotmentModel
    {
        public int Id { get; set; }
        public string LeaveName { get; set; }
        public string Designation { get; set; }
        public int LeavePerMonth { get; set; }
    }
}
