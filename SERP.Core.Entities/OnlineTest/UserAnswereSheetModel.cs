using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.OnlineTest
{
    [Table("StudentAnswereSheet", Schema = "OnlineTest")]
    public class UserAnswereSheetModel:Base<int>
    {
        public int UserTestDetailId { get; set; }
        public int QuestionId { get; set; }
        public int ChooseOptionId { get; set; }
        [NotMapped]
        public bool  IsAttempted { get; set; }
    }
}
