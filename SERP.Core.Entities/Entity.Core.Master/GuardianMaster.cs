using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("GuradianMaster")]
    public class GuardianMaster : Base<int>
    {
        public int StudentId { get; set; }
        public string GuradianName { get; set; }
        public string RelationShip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string GuardianImage { get; set; }
        public int IsVerified { get; set; }
        [NotMapped]
        public string StudentName { get; set; }
    }
}
