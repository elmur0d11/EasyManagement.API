using AutoMapper;
using BCrypt.Net;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using EasyManagement.API.Exceptions;
using EasyManagement.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KeyNotFoundException = EasyManagement.API.Exceptions.KeyNotFoundException;


namespace EasyManagement.API.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserAuthService(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserReadDto> RegisterAsync(User request)
        {
            // Check if username already exists
            if (await _context.users.AnyAsync(u => u.username == request.username))
                throw new KeyNotFoundException("This username is already exist");

            // Hash the password before saving
            request.password_hash = BCrypt.Net.BCrypt.HashPassword(request.password_hash);

            // Save the user to the database
            _context.users.Add(request);
            await _context.SaveChangesAsync();

            // Map to UserReadDto and return
            return _mapper.Map<UserReadDto>(request);
        }
        public async Task<TokenResponseDto?> LoginAsync(User request)
        {
            // Find the user by username
            var user = await _context.users.FirstOrDefaultAsync(u => u.username == request.username);
            if (user is null)
                throw new KeyNotFoundException("Invalid password or username");

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(request.password_hash, user.password_hash))
                throw new KeyNotFoundException("Invalid password or username");

            // Create and return the token response
            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            // Validate the refresh token
            var user = await ValidateRefreshTokenAsync(request.RefreshToken);
            if(user is null)
                throw new BadRequestException("Cannot validate refresh token");

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsyn(user)
            };
        }
        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
        {
            // Find the user by refresh token
            var user = await _context.users.FirstOrDefaultAsync(t => t.refresh_token == refreshToken);
            if (user is null || user.refresh_token != refreshToken || user.refresh_token_expiry_time <= DateTime.UtcNow)
                throw new BadRequestException("Cannot find the user via refresh token");

            return user;
        }
        private string GenerateRefreshToken()
        {
            // Generate a secure random refresh token
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsyn(User user)
        {
            // Generate a new refresh token and save it to the user
            var refreshToken = GenerateRefreshToken();
            user.refresh_token = refreshToken;
            user.refresh_token_expiry_time = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(User user)
        {
            // Define claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role)
            };

            // Create signing credentials
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            // Create credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create the token
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

      
    }
}
