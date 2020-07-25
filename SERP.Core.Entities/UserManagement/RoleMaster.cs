using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("RoleMaster", Schema = "UserManagement")]
    public class RoleMaster:Base<int>
    {
        public string RoleName { get; set; }
    }
}
