using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Data.Entity
{
    public class UserRefreshToken
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
