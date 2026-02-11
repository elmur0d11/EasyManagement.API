namespace EasyManagement.API.Dto
{
    public class UserLoginDto
    {
        public string username { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
    }
}
