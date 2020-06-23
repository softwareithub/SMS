using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.FeeDetails
{
    public class OnlineFeeModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool  IsCompleteCourseFee { get; set; }
        public int SubjectId { get; set; }
    }
}
