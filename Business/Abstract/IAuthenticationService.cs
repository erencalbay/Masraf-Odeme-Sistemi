using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthenticationService
    {
        Task<Token> LoginAsync(Login login);
        Task<Token> LoginByRefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
