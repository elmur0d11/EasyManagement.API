namespace EasyManagement.API.Dto
{
    public class AccountReadDto
    {
        public string username { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = "User";
    }
}
