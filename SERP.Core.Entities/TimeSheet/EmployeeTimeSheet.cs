using SERP.Core.Entities.Entity.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Core.Entities.TimeSheet
{
    [Table("EmployeeTimeSheet", Schema = "TimeSheet")]
    public class EmployeeTimeSheet: Base<int>
    {
        [Required(ErrorMessage ="Please select class")]
        public int ClassId { get; set; }

        [Required(ErrorMessage ="Please select section")]
        public int SectionId { get; set; }

        [Required(ErrorMessage ="Please select subject")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Please select period")]
        public int PeriodId { get; set; }

        [Required(ErrorMessage = "Please select lession")]
        public int LessionId { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        public string Desription { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }
    }
}
