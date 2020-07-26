using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.UserManagement
{
    public class StudentAccountModel
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int BatchId { get; set; }
        public string ImagePath { get; set; }
    }
}
