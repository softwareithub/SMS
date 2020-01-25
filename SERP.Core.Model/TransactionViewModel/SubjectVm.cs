using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Entities.Entity.Core.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.TransactionViewModel
{
    public class SubjectVm
    {
        public int SubjectId { get; set; }
        public string CourseName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public int IsElective { get; set; }
    }
    
}
