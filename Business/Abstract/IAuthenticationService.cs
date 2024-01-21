using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    // Auth service için gerekli olan methodları interface olarak tanımladık.
    public interface IAuthenticationService
    {
        Task<Token> LoginAsync(Login login);
        Task<Token> LoginByRefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
