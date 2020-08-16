using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.UserManagement
{
    public class QuickLinkVm
    {
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public bool IsMapped { get; set; } = false;
        public int RoleId { get; set; }
    }
}
