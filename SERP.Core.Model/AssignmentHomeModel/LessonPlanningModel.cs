using SERP.Core.Entities.LessionMaster;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.AssignmentHomeModel
{
    public class LessonPlanningModel
    {
        public int LessonId { get; set; }
        public LessonMaster LessonMasterModel {get; set;}
        public string SubjectName { get; set; }
    }
}
