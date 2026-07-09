using EasyManagement.API.Dto;
using EasyManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyManagement.API.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAccount(AccountUpdateDto request)
        {
            // Extract user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse the user ID from the claim
            int userId = int.Parse(userIdClaim.Value);
            // Call the service to update the account
            var result = await _accountService.UpdateAccount(userId, request);
            _logger.LogInformation("Account updated successfully. User ID: {userId}", userId);
            // Return the result
            return Ok(result);
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDto request)
        {
            // Extract user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            // Parse the user ID from the claim
            int userId = int.Parse(userIdClaim.Value);
            // Call the service to update the password
            var result = await _accountService.UpdatePassword(userId, request);
            _logger.LogInformation("Password updated successfully. User ID: {userId}", userId);
            // Return the result
            return Ok(result);
        }
    }
}
