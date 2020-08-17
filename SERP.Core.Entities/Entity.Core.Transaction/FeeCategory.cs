using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeCategory", Schema = "TransactionSch")]
    public class FeeCategory :Base<int>
    {
        [Required(ErrorMessage ="Fee category name is required")]
        [Display(Name ="Category Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Fee Category code is required.")]
        [Display(Name ="Category Code")]
        [MaxLength(100)]
        public string Code { get; set; }
        public int SessionId { get; set; }
    }
}
