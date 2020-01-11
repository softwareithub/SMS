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
        [Required(ErrorMessageResourceType = typeof(ValidaionMessage), ErrorMessageResourceName = "AcademicStartDate")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(ValidaionMessage), ErrorMessageResourceName = "AcademicEndDate")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
