using SERP.Core.Entities.Entity.Core;
using SERP.Core.Entities.Entity.Core.ExamDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.UI.Models
{
    public class QuestionVm:Base<int>
    {
        public QuestionModel QuestionModel { get; set; }
        public OptionMaster OptionMasters { get; set; }
    }
}
