using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SERP.Core.Entities.Entity.Core
{
    public class Base<T>
    {
        [Key]
        public T Id { get; set; }
        public int IsActive { get; set; } = 1;
        public int IsDeleted { get; set; } = 0;
        public int CreatedBy { get; set; } = 1;
        public DateTime CreatedDate { get; set; } = DateTime.Now.Date;
        public int UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; } = null;
    }
}
