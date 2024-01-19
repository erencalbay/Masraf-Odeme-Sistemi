using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Entity
{
    public abstract class BaseEntity
    {
        public int InsertUserNumber { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdateUserNumber { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool isActive { get; set; }
    }
}
