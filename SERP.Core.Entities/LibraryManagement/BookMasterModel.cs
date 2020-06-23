using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("BookMaster", Schema = "LibraryManagement")]
    public class BookMasterModel:Base<int>
    {
        public string BookName { get; set; }
        public string PublisherName { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }
        public string Author { get; set; }
    }
}
