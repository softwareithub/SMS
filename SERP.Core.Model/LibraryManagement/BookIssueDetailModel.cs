using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace SERP.Core.Model.LibraryManagement
{
    public class BookIssueDetailModel
    {
        public int Id { get; set; }
        public int BookItemId { get; set; }
        public string BookName { get; set; }
        public string BookItemName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public string IssueTo { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }
        [DataType(DataType.Currency, ErrorMessage ="Invalid Fine Amount")]
        public decimal FineAmount { get; set; }
        public string FineReason { get; set; }
        public int BookCondition { get; set; }
        public string Remark { get; set; }
        [DataType(DataType.Currency, ErrorMessage ="Invalid Discount Amount")]
        public decimal DiscountAmount { get; set; }
        public string DiscountReason { get; set; }
        [Required(ErrorMessage ="Actual return date is required.")]
        [DataType(DataType.Date)]
        public DateTime ActualReturnDate { get; set; }
        public string BookIssueDate { get; set; }
        public string BookReturnDate { get; set; }
        public string BookActualReturnDate { get; set; }
    }
}
