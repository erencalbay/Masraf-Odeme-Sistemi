using Data.Entity;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Abstract
{
    public interface ITokenService
    {
        Task<Token> CreateToken(User user, List<string> roles);
    }
}
