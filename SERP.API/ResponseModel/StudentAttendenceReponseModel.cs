using SERP.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.ResponseModel
{
    public class StudentAttendenceReponseModel : BaseModel<int>
    {
        public string RollCode { get; set; }
        public string Registration { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentPhoto { get; set; }
        public DateTime AttendenceDate { get; set; }
        public string AttendenceType { get; set; }
    }
}
