using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs
{
    public class UpdateTasksStatusDTO
    {
        [Required]
        public bool IsCompleted { get; set; }

    }
}
