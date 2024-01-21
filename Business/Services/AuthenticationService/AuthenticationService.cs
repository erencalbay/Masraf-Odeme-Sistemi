using Business.Abstract;
using Data.DbContextCon;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Entity;

namespace Business.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly VdDbContext _dbcontext;

        public AuthenticationService(ITokenService tokenService, VdDbContext dbcontext)
        {
            _tokenService = tokenService;
            _dbcontext = dbcontext;
        }

        // Token yenileme işlemleri
        public async Task<Token> LoginAsync(Login login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var user = await _dbcontext.Set<User>().Where(x => x.UserNumber == login.UserNumber).Include(x => x.Roles).FirstOrDefaultAsync();


            if (user == null) throw new Exception("UserNumber is wrong");

            var roleIds = user.Roles.Select(x => x.RoleId).ToList();
            var roles = await _dbcontext.Set<Role>().Where(x => roleIds.Contains(x.Id)).ToListAsync();

            var token = await _tokenService.CreateToken(user, roles.Select(x => 
            {
                return x.Name;
            }).ToList());

            var userRefreshToken = await _dbcontext.Set<UserRefreshToken>().Where(rt => rt.UserId == user.UserNumber).FirstOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _dbcontext.Set<UserRefreshToken>().AddAsync(new UserRefreshToken { UserId = user.UserNumber, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _dbcontext.SaveChangesAsync();

            return token;
        }

        // Tokenın yenilenme işlemi
        public async Task<Token> LoginByRefreshTokenAsync(string refreshToken)
        {
            var existReFreshToken = await _dbcontext.Set<UserRefreshToken>().Where(rt => rt.Code.Equals(refreshToken)).FirstOrDefaultAsync();

            if (existReFreshToken == null) throw new Exception("Refresh token is invalid");

            if (DateTime.UtcNow > existReFreshToken.Expiration) throw new Exception("Refresh token expired");

            var user = await _dbcontext.Set<User>().Where(x => x.UserNumber == existReFreshToken.UserId).Include(x => x.Roles).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Data Binding Error Check AuthenditcationService relation userId -> refreshToken");

            var roleIds = user.Roles.Select(x => x.RoleId).ToList();
            var roles = await _dbcontext.Set<Role>().Where(x => roleIds.Contains(x.Id)).ToListAsync();

            var token = await _tokenService.CreateToken(user, roles.Select(x =>
            {
                return x.Name;
            }).ToList());

            existReFreshToken.Code = token.RefreshToken;
            existReFreshToken.Expiration = token.RefreshTokenExpiration;

            await _dbcontext.SaveChangesAsync();

            return token;
        }

        // Refresh token ile revokelama
        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _dbcontext.Set<UserRefreshToken>().Where(rt => rt.Code.Equals(refreshToken)).FirstOrDefaultAsync();
            if (existRefreshToken == null) throw new Exception("Refresh token not found");

            _dbcontext.Remove(existRefreshToken);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
