using EasyManagement.API.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyManagement.API.Dto
{
    public class TaskPriorityUpdate
    {
        public string Priority { get; set; } = string.Empty;
    }
}
