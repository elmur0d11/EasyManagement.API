namespace EasyManagement.API.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password_hash { get; set; } = string.Empty;
        public string role { get; set; } = "User";
        public string? refresh_token { get; set; }
        public DateTime refresh_token_expiry_time { get; set; }

    }
}
