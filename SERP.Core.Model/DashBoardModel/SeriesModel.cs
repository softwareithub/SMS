using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class SeriesModel<T>
    {
        public string name { get; set; }
        public Dictionary<string,int> data { get; set; }
    }
}
