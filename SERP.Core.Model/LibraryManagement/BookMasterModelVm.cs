using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Model.LibraryManagement
{
    public class BookMasterModelVm
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string CourseName { get; set; }
        public string SubjectName { get; set; }
        public string PublisherName { get; set; }
        public string Language { get; set; }
        public string Edition { get; set; }
        public string  Description { get; set; }
        public string ImagePath { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int TotalCount { get; set; }
    }
}
