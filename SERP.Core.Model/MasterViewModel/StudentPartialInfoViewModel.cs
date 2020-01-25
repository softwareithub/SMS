using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.MasterViewModel
{
    public class StudentPartialInfoViewModel
    {
        public int Id { get; set; }
        public string RollCode { get; set; }
        public string Registration { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Religion { get; set; }
        public string Category { get; set; }
        public string FatherEmail { get; set; }
        public string StudentEmail { get; set; }
        public string FatherPhone { get; set; }
        public string EmergencyPhone { get; set; }
        public string StudentPhone { get; set; }
        public string MotherPhone { get; set; }
        public string P_Address { get; set; }
        public string C_Address { get; set; }
        public string StudentPhoto { get; set; }
        public string ParentPhoto { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
