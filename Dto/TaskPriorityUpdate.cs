using EasyManagement.API.Enums;
using EasyManagement.API.Models;
using System.ComponentModel.DataAnnotations;

namespace EasyManagement.API.Dto
{
    public class TaskPriorityUpdate
    {
        public TaskPriority Priority { get; set; }
    }
}
