using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SERP.Core.Model.QuickSearchModel
{
    public class StudentQuickModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string  CourseName { get; set; }
        public string BatchName { get; set; }
        public string RollCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string ReligionName { get; set; }
        public string FeeCategory { get; set; }
        public string FatherEmail { get; set; }
        public string EmergencyPhone { get; set; }
        public string StudentPhone { get; set; }
        public string MotherPhone { get; set; }
        public string PAddress { get; set; }
        public string CAddress { get; set; }
        public string StudentPhoto { get; set; }
        public string ParentPhoto { get; set; }
    }
}
