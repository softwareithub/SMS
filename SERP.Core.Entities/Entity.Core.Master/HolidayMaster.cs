using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    public class HolidayMaster:Base<int>
    {
        public string DayName { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public string HolidayDescription { get; set; }
        public string HolidayName { get; set; }
        public string ForClass { get; set; }
    }
}
