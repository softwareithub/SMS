using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Model
{
    public class StudentHomeWorkModel:Base<int>
    {
        public string HomeWorkName { get; set; }
        public DateTime HomeWorkDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime ActualSubmissionDate { get; set; }
        public string Grade { get; set; }
        public string Notes { get; set; }
    }
}
