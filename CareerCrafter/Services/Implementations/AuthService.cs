using CareerCrafter.DTOs;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using CareerCrafter.Auth;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly CareerCrafterDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(CareerCrafterDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<object?> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

            if (user == null)
                return null;

            var token = _jwtService.GenerateToken(user.Username, user.Role);

            return new
            {
                Token = token,
                user.Username,
                user.Role,
                Message = "Login successful"
            };
        }

        public async Task<object> RegisterAsync(RegisterDTO dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (existingUser != null)
                return new { Error = "Username already exists." };

            var newUser = new UserInfo
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new
            {
                Message = "User registered successfully",
                newUser.Username,
                newUser.Email,
                newUser.Role
            };
        }
    }
}
