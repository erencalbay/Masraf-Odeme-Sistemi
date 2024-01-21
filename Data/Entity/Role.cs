using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Data.Entity
{
    // Role entityleri
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<RoleUser> Users { get; set; }
    }
}
