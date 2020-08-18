using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("GuradianMaster")]
    public class GuardianMaster : Base<int>
    {
        [Required(ErrorMessage = "Please select student")]
        public int StudentId { get; set; } = default;

        [Required(ErrorMessage = "Please enter guardian name")]
        public string GuradianName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select relationship")]
        public string RelationShip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter phone number")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please upload guardian picture")]
        public string GuardianImage { get; set; } = string.Empty;
        public int IsVerified { get; set; } = default;
        [NotMapped]
        public string StudentName { get; set; }
        public int SessionId { get; set; }
    }
}
