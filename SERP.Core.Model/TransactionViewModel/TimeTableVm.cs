using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class TimeTableVm
    {
        public string DayName { get; set; }
        public List<PeriodVm> PeriodModels { get; set; }
    }
}
