﻿using Business.Abstract;
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

        public async Task<Token> LoginAsync(Login login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var user = await _dbcontext.Set<User>().Where(x => x.UserNumber == login.UserNumber).FirstOrDefaultAsync();

            if (user == null) throw new Exception("UserNumber is wrong");

            var token = await _tokenService.CreateToken(user);

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
        public async Task<Token> LoginByRefreshTokenAsync(string refreshToken)
        {
            var existReFreshToken = await _dbcontext.Set<UserRefreshToken>().Where(rt => rt.Code.Equals(refreshToken)).FirstOrDefaultAsync();

            if (existReFreshToken == null) throw new Exception("Refresh token is invalid");

            if (DateTime.UtcNow > existReFreshToken.Expiration) throw new Exception("Refresh token expired");

            var user = await _dbcontext.Set<User>().Where(x => x.UserNumber == existReFreshToken.UserId).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Data Binding Error Check AuthenditcationService relation userId -> refreshToken");

            var token = await _tokenService.CreateToken(user);

            existReFreshToken.Code = token.RefreshToken;
            existReFreshToken.Expiration = token.RefreshTokenExpiration;

            await _dbcontext.SaveChangesAsync();

            return token;
        }

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