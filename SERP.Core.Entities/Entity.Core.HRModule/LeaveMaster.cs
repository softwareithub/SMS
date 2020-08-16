using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.HRModule
{
    [Table("LeaveMaster")]
    public class LeaveMaster: Base<int>
    {
        [Required(ErrorMessage = "Please enter leave name")]
        public string LeaveName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter leave code")]
        public string LeaveCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
