using jwt.netCore.API.Data;
using jwt.netCore.API.Models;
using jwt.netCore.API.Models.DTOs;
using jwt.netCore.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt.netCore.API.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;


        public AuthenticationService(DataContext context, IConfiguration configuration, IUsuarioService usuarioService)
        {
            _context = context;
            _configuration = configuration;
            _usuarioService = usuarioService;
        }

        public async Task<ServiceResponse<string>> Authenticate(LoginUser loginUser)
        {
            var sr = new ServiceResponse<string>();
            Usuario user = await _context.DD_Usuarios.FirstOrDefaultAsync(x => x.usr_Login.ToLower().Equals(loginUser.LoginOrEmail.ToLower())) ??
                           await _context.DD_Usuarios.FirstOrDefaultAsync(x => x.usr_Email.ToLower().Equals(loginUser.LoginOrEmail.ToLower()));
            if (user == null)
            {
                sr.Success = false;
                sr.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(loginUser.Password, user.usr_PasswordHash, user.usr_PasswordSalt))
            {
                sr.Success = false;
                sr.Message = "Wrong password.";
            }
            else
            {
                sr.Message = user.usr_Login + " is authenticate.";
                sr.Success = true;
                sr.Data = CreateToken(user);
            }
            return sr;
        }

        public async Task<ServiceResponse<int>> Register(AddUsuarioDto user, string password)
        {
            var sr = new ServiceResponse<int>();
            if (await UserExists(user.usr_Login))
            {
                sr.Success = false;
                sr.Message = "User already exists.";
                return sr;
            }

            //CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            //user.usr_PasswordHash = passwordHash;
            //user.usr_PasswordSalt = passwordSalt;

            await _usuarioService.AddUsuario(user);
            //await _context.SaveChangesAsync();
            sr.Success = true;
            sr.Message = "User registred.";
            return sr;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.DD_Usuarios.AnyAsync(x => x.usr_Login.ToLower() == username.ToLower()))
                return true;

            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        private string CreateToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.usr_Id.ToString()),
                new Claim(ClaimTypes.Name, user.usr_Name)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
