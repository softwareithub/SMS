using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.UserManagement
{
    [Table("ModuleMaster")]
    public class ModuleMaster :Base<int>
    {
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
