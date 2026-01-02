using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs
{
    public class UpdateTasksDTO
    {
        [Required]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
