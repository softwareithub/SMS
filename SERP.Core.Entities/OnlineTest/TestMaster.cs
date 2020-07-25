using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.OnlineTest
{
    [Table("TestMaster", Schema = "OnlineTest")]
    public class TestMaster:Base<int>
    {
        [Required(ErrorMessage ="Test name is required")]
        public string TestName { get; set; }
        [Required(ErrorMessage ="Test Rule and Regulation is required")]
        public string Regulation { get; set; }
        public string TestTimeLimit { get; set; }
        public int NoOfQuestion { get; set; }
        public DateTime TestDateTime { get; set; }
        [Required(ErrorMessage ="Please select Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage ="Please select Batch")]
        public int BatchId { get; set; }


    }
}
