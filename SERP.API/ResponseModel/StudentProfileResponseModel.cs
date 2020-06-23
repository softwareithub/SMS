using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SERP.API.ResponseModel
{
    public class StudentProfileResponseModel :Base<int>
    {
        public string  StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string RegistrationNumber { get; set; }
        public string RollCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string StudentPhoto { get; set; }
        public string ParentPhoto { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Category { get; set; }
        public string FatherEmail { get; set; }
        public string StudentEmail { get; set; }
        public string FatherPhone { get; set; }
        public string StudentPhone { get; set; }
        public string MotherPhone { get; set; }
        public string P_Address { get; set; }
        public string C_Address { get; set; }
        public string Religion { get; set; }
    }
}
