using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.UserManagement
{
    public class UserLoginModel
    {
        public int Id { get; set; }
        public string   EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IsExpired { get; set; }
        public int IsLocked { get; set; }
        public int IsActive { get; set; }
        public string EmployeeCode { get; set; }
        public string StudentName { get; set; }
        public string RegistrationNumber { get; set; }
        public int RoleId { get; set; }
    }
}
