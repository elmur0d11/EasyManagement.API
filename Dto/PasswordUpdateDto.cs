namespace EasyManagement.API.Dto
{
    public class PasswordUpdateDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
