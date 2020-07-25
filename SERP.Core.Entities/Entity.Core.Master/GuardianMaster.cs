using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("GuradianMaster")]
    public class GuardianMaster : Base<int>
    {
        public int StudentId { get; set; } = default;
        public string GuradianName { get; set; } = string.Empty;
        public string RelationShip { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string GuardianImage { get; set; } = string.Empty;
        public int IsVerified { get; set; } = default;
        [NotMapped]
        public string StudentName { get; set; }
    }
}
