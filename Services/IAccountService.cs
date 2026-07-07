using EasyManagement.API.Dto;

namespace EasyManagement.API.Services
{
    public interface IAccountService
    {
        Task<AccountReadDto> UpdateAccount(int userId, AccountUpdateDto request);
        Task<AccountReadDto> UpdatePassword(int userId, PasswordUpdateDto request);
    }
}
    