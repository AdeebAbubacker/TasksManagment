using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment.Application.Features.Tasks.Queries.GetTasksList
{
    public class TasksListDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        public String OwnerUserId  { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
