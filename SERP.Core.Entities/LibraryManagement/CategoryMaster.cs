using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
    [Table("CategoryMaster", Schema = "LibraryManagement")]
    public class CategoryMaster: Base<int>
    {
        [Required(ErrorMessage ="Category name is required.")]
        [MaxLength(50,ErrorMessage ="Name is too Large")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int SessionId { get; set; }
    }
}
