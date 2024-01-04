using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using NewCourse.Entities.Entities;
using NewCourse.Entities.DTOs;
using NewCourse.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace NewCourse.Services
{
    public class AccountService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly AccountUtility _accountUtility;
        public AccountService(IConfiguration configuration, AppDbContext context, AccountUtility accountUtility)
        {
            _configuration = configuration;
            _context = context;
            _accountUtility = accountUtility;
        }
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user is not null)
                throw new Exception("User already exist");

            _accountUtility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userToAdd = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user is null)
                throw new Exception("User was not found");

            if (!_accountUtility.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Username or Password is incorrect");

            user.TokenCreated = DateTime.Now;
            user.TokenExpires = DateTime.Now.AddDays(1);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            string token = _accountUtility.CreateToken(user);

            return token;
        }



        public async Task<string> LogOut()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.TokenExpires > DateTime.Now);
            if (user is not null)
            {
                user.TokenExpires = DateTime.Now;
                await _context.SaveChangesAsync();
                _accountUtility.CreateToken(user);
                return "Signed Out Succesfuly";
            }
            
            throw new Exception("Token is Not Valid");
        }
    }
}


