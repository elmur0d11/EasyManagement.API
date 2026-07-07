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
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAccount(AccountUpdateDto request)
        {
            // Extract user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            // Parse the user ID from the claim
            int userId = int.Parse(userIdClaim.Value);
            // Call the service to update the account
            var result = await _accountService.UpdateAccount(userId, request);
            // Return the result
            return Ok(result);
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDto request)
        {
            // Extract user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null)
                return Unauthorized("User ID not found in token.");
            // Parse the user ID from the claim
            int userId = int.Parse(userIdClaim.Value);
            // Call the service to update the password
            var result = await _accountService.UpdatePassword(userId, request);
            // Return the result
            return Ok(result);
        }
    }
}
