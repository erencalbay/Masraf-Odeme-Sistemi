using Business.Abstract;
using Business.Services.Sign;
using Data.DbContextCon;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schema;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOptions _customTokenOptions;
        private readonly VdDbContext _vdDbContext;

        public TokenService(IOptions<CustomTokenOptions> options, VdDbContext vdDbContext)
        {
            _customTokenOptions = options.Value;
            _vdDbContext = vdDbContext;
        }

        public async Task<Token> CreateToken(User user)
        {
            // perapare token option from configuration
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

            // get signging algorithm for token with our keys
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create token operations
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: await GetClaims(user, _customTokenOptions.Audiences));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            // we convert to custom dto for open the outside
            var tokenDto = new Token()
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_customTokenOptions.RefreshTokenExpiration)
            };

            return tokenDto;
        }

        private string CreateRefreshToken()
        {
            //32 byte random string data

            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);

        }

        private async Task<IEnumerable<Claim>> GetClaims(User user, List<string> audiences)
        {
            // token's payload is claims

            // if you want you can write "id","1" - "email","example.com" ...
            // but if you want the claims to pair with identity lib you can use const type 
            // For example you can use in controller User.Identity.Name it pair to ClaimTypes.Name
            // otherwise you must User.claims(c => c.type == "myUserName")
            var userClaimList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserNumber.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("usernumber", user.UserNumber.ToString()), // we dont have const arch type so we wrote manuel
            };
            // the last one for like a pk
            // this claims about user, after created jwt they added to payload

            userClaimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            // we apply to format to arch can find Audiencess 


            // now we get user roles and add to claims
            var userRoles = await _vdDbContext.Set<User>().Include(x => x.Roles).Where(x => x.UserNumber == user.UserNumber).FirstOrDefaultAsync();
            userClaimList.AddRange(userRoles.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));


            return userClaimList;
        }
    }
}
