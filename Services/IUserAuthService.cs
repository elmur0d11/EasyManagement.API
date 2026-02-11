using EasyManagement.API.Dto;
using EasyManagement.API.Models;

namespace EasyManagement.API.Services
{
    public interface IUserAuthService
    {
        Task<UserReadDto> RegisterAsync(User request);
        Task<TokenResponseDto?> LoginAsync(User request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
