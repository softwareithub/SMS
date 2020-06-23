using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.AssignmentHomeModel
{
    public class HomeWorkAllocationModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string RollCode { get; set; }
        public string ImagePath { get; set; }
        public int HomeWorkId { get; set; }
        public DateTime SubMissionDate { get; set; }
        public int IsSubmitted { get; set; }
        public string Grade { get; set; }
        public string FeedBack { get; set; }

    }
}
