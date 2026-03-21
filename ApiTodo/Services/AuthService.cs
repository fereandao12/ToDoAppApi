using ApiTodo.Data;
using ApiTodo.DTOs;
using ApiTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiTodo.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> RegisterAsync(UserAuthDto authDto)
        {
            if(await _context.Users.AnyAsync(u => u.UserName == authDto.Username)) return false;

            var user = new User
            {
                UserName = authDto.Username
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, authDto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AuthResponseDto?> AuthenticateAsync(UserAuthDto authDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == authDto.Username);
            if(user == null) return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authDto.Password);
            if (verificationResult == PasswordVerificationResult.Failed) return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration["Jwt:Key"]
    ??       throw new InvalidOperationException("La clave 'Jwt:Key' no está configurada en appsettings.json.");
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresInMinutes = Convert.ToInt32(_configuration["Jwt:DurationInMinutes"])
            };
        }

        
    }
}
