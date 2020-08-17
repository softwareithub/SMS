using SERP.Core.Entities.Entity.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Core.Entities.TransactionLog
{
    [Table("LogMaster",Schema = "TransactionLog")]
    public class LogMaster:Base<int>
    {
        public string Message { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string CompleteLog { get; set; }

        public int SessionId { get; set; }
    }
}
