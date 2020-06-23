using SERP.Core.Entities.Entity.Core.ExamDetail;
using SERP.Core.Entities.Entity.Core.Master;
using SERP.Core.Model.MasterViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.ExamModel
{
    public class StudentMarkSheetVm
    {
        public InstituteMaster InstituteModel { get; set; }
        public StudentPartialInfoViewModel StudentInfoModel { get; set; }
        public Exam ExamModel { get; set; }
        public List<StudentMarkAllocationVm> MarkAllocationVms { get; set; }
    }
}
