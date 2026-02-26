using AutoMapper;
using BCrypt.Net;
using EasyManagement.API.Data;
using EasyManagement.API.Dto;
using EasyManagement.API.Models;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserAuthService _authService;
        public UserAuthController(IMapper mapper,IUserAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserCreateDto request)
        {
            // Map DTO to User model
            var userModel = _mapper.Map<User>(request);

            // Register user
            var user = await _authService.RegisterAsync(userModel);
            if (user == null)
                return BadRequest("User already exists.");

            // Map User model to Read DTO
            return Created("", user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            // Map DTO to User model
            var userModel = _mapper.Map<User>(request);
            var result = await _authService.LoginAsync(userModel);
            if(result is null)
                return BadRequest("Invalid username or password.");

            // Map User model to Read DTO
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            // Refresh tokens
            var result = await _authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return BadRequest("Invalid refresh token.");

            // Map User model to Read DTO
            return Ok(result);
        }

    }
}
