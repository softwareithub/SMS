using SERP.Core.Entities.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.UserManagement
{
    public class UserAccessRightModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public int EmployeeId { get; set; }
        public int SubModuleId { get; set; }
        public int ReadAccess { get; set; }
        public int CreateAccess { get; set; }
        public int UpdateAccess { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ClassName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
