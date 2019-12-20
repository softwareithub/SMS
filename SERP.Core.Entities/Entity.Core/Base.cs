using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Core.Entities.Entity.Core
{
    public class Base<T>
    {
        public T Id { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
