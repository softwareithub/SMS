using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("Authenticate",Schema = "UserManagement")]
    public class Authenticate:Base<int>
    {
        [Required(ErrorMessage ="Please enter user name.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter passwors.")]
        public string Password { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public int IsExpired { get; set; }
        public int Attempt { get; set; }
        public int IsLocked { get; set; }
        public int EmployeeId { get; set; }
        public int StudentId { get; set; }
    }
}
