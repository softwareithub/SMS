using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("AcademicMaster", Schema = "Master")]
    public class AcademicMaster : Base<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
