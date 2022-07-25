using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SERP.API.Model
{
    public class MappedTecherModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string SubjectName { get; set; }
    }
}
