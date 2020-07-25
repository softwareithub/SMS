using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("BookTransaction", Schema = "LibraryManagement")]
   public  class BookTransaction:Base<int>
    {
        [Required(ErrorMessage ="Please select book to issue")]
        public int BookItemId { get; set; }
        [Required(ErrorMessage ="Please select User")]
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public int BookStatusId { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Issue Date is required.")]
        public DateTime IssueDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public string BarCodeImage { get; set; }
        public decimal FineAmount { get; set; }
        public string FineReason { get; set; }
        public decimal FineDiscountAmount { get; set; }
        public string DiscountReason { get; set; }
    }
}
