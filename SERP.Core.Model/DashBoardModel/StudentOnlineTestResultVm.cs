using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.DashBoardModel
{
    public class StudentOnlineTestResultVm
    {
        public string StudentName { get; set; }
        public string TestName { get; set; }
        public string TestTimeLimit { get; set; }
        public int NoOfQuestion { get; set; }
        public DateTime ExaminationDate { get; set; }
    }
}
