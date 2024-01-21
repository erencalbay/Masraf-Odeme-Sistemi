using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Entity
{
    // Bazı entitylerde id bulunmuyor, o yüzden ayrı olarak bir class oluşturduk.
    public abstract class BaseEntityWithId : BaseEntity
    {
        public int Id { get; set; }
    }
}
