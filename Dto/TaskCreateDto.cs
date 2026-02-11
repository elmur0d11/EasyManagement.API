namespace EasyManagement.API.Dto
{
    public class TaskCreateDto
    {
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string priority { get; set; } = "Low";
        public string room_code { get; set; } = string.Empty;
    }
}
