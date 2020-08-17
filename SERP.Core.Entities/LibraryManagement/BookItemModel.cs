using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("BookItem", Schema = "LibraryManagement")]
    public class BookItemModel: Base<int>
    {
        public int BookId { get; set; } = default;
        public string BarCode { get; set; } = string.Empty;
        public string ISBNNumber { get; set; } = string.Empty;
        public string BINShelf { get; set; } = string.Empty;
        public string BookBarCode { get; set; } = string.Empty;
        public int BookStatus { get; set; } = default;
        public int SessionId { get; set; }
    }
}
