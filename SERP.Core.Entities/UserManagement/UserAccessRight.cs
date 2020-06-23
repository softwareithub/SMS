using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("UserAccessRight")]
    public class UserAccessRight: Base<int>
    {
        public int ModuleId { get; set; }
        public int EmployeeId { get; set; }
        public int SubModuleId { get; set; }
        public int ReadAccess { get; set; }
        public int CreateAccess { get; set; }
        public int UpdateAccess { get; set; }
    }
}
