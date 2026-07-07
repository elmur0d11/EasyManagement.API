namespace EasyManagement.API.Dto
{
    public class UserReadDto
    {
        public string username { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
