namespace EasyManagement.API.Dto
{
    public class TaskReadDto
    {
        public string title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string priority { get; set; } = "Low";
        public string status { get; set; } = "In Progress";
        public DateTime created_at { get; set; }
    }
}
