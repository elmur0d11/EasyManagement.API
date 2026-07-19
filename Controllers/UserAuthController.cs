using AutoMapper;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;
using EasyManagement.API.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EasyManagement.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserAuthService _authService;
        private readonly ILogger<UserAuthController> _logger;
        public UserAuthController(IMapper mapper,IUserAuthService authService, ILogger<UserAuthController> logger)
        {
            _mapper = mapper;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserCreateDto request)
        {
            _logger.LogInformation("Register request recieved for email: {email}", request.Email);
            // Map DTO to User model
            var userModel = _mapper.Map<User>(request);
            // Register user
            var user = await _authService.RegisterAsync(userModel);
            _logger.LogInformation("User successfully registered. Username: {username}", request.Username);
            // Map User model to Read DTO
            return Created("", user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            _logger.LogInformation("Login request recieved for Username: {username}", request.Username);
            // Map DTO to User model
            var userModel = _mapper.Map<User>(request);
            var result = await _authService.LoginAsync(userModel);
            _logger.LogInformation("User logged successfully. Username: {username}", request.Username);
            // Map User model to Read DTO
            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            _logger.LogInformation("Refresh token request received.");
            // Refresh tokens
            var result = await _authService.RefreshTokensAsync(request);
            _logger.LogInformation("Token refreshed successfully.");
            // Map User model to Read DTO
            return Ok(result);
        }

    }
}
