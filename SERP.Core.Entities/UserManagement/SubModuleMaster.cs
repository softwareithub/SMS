using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("SubModule")]
    public class SubModuleMaster:Base<int>
    {
        public int ModuleId { get; set; }
        public string SubModuleName { get; set; }
        public string ClassName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
