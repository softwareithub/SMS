using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("BookStatus", Schema = "LibraryManagement")]
    public class BookStatusModel: Base<int>
    {
        [Required(ErrorMessage ="Book Status is required.")]
        [DisplayName("Status")]
        public string StatusName { get; set; }
        public string ColorCode { get; set; }
        public int SessionId { get; set; }
    }
}
