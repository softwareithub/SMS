using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.UserManagement
{
    public class MenuSubMenuVm
    {
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ModuleClass { get; set; }
        public string SubModuleClass { get; set; }
        public int ReadAccess { get; set; }
        public int CreateAccess { get; set; }
        public int UpdatedAccess { get; set; }
        public string ModuleClassName { get; set; }
        public int ModuleDisplayOrder { get; set; }
        public int SubModuleDisplayOrder { get; set; }
        public string LoginUserName { get; set; }
    }
}
