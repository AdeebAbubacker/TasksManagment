using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Domain.Entities;

namespace TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList


{
    internal static class mapperExtensions
{
    internal static TasksListDTO ToDto(this TaskItem taskItem)
    {
        var dto = new TasksListDTO
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            CreatedAt = taskItem.CreatedAt,
            Description = taskItem.Description,
            DueDate = taskItem.DueDate,
            OwnerUserId = taskItem.OwnerUserId,
            IsCompleted = taskItem.IsCompleted,

        };
        return dto;
    }
}
}
