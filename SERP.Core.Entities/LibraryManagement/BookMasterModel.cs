using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("BookMaster", Schema = "LibraryManagement")]
    public class BookMasterModel:Base<int>
    {
        [Required(ErrorMessage ="Title is required")]
        [MaxLength(200, ErrorMessage ="Title is too large.")]
        public string TitleName { get; set; }
        [Required(ErrorMessage ="Author name is required")]
        [MaxLength(200,ErrorMessage ="Author name is too large.")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage ="Category is required.")]
        public int CategoryId { get; set; }
        public int CourseId { get; set; } = default;
        public int SubjectId { get; set; } = default;
        public string PublisherName { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Edition { get; set; } = default;
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        [DataType(DataType.Currency, ErrorMessage = "The Cost price must be currency")]
        public decimal CostPrice { get; set; } = default;
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; } = default(DateTime);
        [NotMapped]
        [Range(0,1000)]
        public int TotalBookCount { get; set; }

        [NotMapped]
        public bool IsBarCodeGenerated { get; set; }

        public int SessionId { get; set; }
    }
}
