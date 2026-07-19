using EasyManagement.API.Enums;

namespace EasyManagement.API.Dto
{
    public class AccountUpdateDto
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
