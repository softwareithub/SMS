using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.LibraryReport
{
    public class BookDetailReport
    {
        public int BookCount { get; set; }
        public string TitleName { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public string StatusName { get; set; }
        public string  ImagePath { get; set; }
        public decimal UnitPrice { get; set; }
        public string CourseName { get; set; }
        public string SubjectName { get; set; }
    }
}
