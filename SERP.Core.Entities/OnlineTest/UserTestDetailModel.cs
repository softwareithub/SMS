using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.OnlineTest
{
    [Table("UserTestDetail", Schema = "OnlineTest")]
    public class UserTestDetailModel:Base<int>
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public DateTime DateOfExamination { get; set; }
        public string TestStatus { get; set; }
    }
}
