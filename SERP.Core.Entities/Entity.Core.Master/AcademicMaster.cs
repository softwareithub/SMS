using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Core.Entities.Entity.Core.Master
{
    [Table("AcademicMaster", Schema = "Master")]
    public class AcademicMaster : Base<int>
    {
        [Required(ErrorMessageResourceType = typeof(ValidaionMessage), ErrorMessageResourceName ="AcademinName")]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
