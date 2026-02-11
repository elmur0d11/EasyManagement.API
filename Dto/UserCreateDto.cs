namespace EasyManagement.API.Dto
{
    public class UserCreateDto
    {
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
        public string role { get; set; } = "Guest";
    }
}
