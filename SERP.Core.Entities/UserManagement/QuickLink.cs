using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("QuickLink", Schema = "Master")]
    public class QuickLink:Base<int>
    {
        public int SubModuleId { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int SessionId { get; set; }
    }
}
