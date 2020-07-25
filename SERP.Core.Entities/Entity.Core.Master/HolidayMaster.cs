using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    public class HolidayMaster:Base<int>
    {
        public string DayName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = default(DateTime);
        public int Year { get; set; } = default;
        public string HolidayDescription { get; set; } = string.Empty;
        public string HolidayName { get; set; } = string.Empty;
        public string ForClass { get; set; } = string.Empty;
    }
}
