using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class ChartModel<T>
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string[] XAxisCategories { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string YAxisTooltipValueSiffix { get; set; }
        public ICollection<SeriesModel<T>> Series { get; set; }

    }
}
