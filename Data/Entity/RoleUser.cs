using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Data.Entity
{
    public class RoleUser
    {
        [Key]
        public int RoleId { get; set; }
        [Key]
        public int UserNumber { get; set; }
        public Role Role { get; set; }
        [ForeignKey("UserNumber")]
        public User User { get; set; }
    }
}
