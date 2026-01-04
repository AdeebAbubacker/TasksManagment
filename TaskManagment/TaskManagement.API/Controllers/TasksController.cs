using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DTOs;
using TaskManagement.API.Utilities;
using TaskManagment.Application.Features.Tasks.Commands.CreateTasks;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasks;
using TaskManagment.Application.Features.Tasks.Commands.UpdateTasksStatus;
using TaskManagment.Application.Features.Tasks.Queries.GetMyTasksList;
using TaskManagment.Application.Features.Tasks.Queries.GetTasksList;
using TaskManagment.Application.Utilities;



namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator mediator;

        public TasksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // Create Task (Users only)
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post(CreateTaskDto createTaskDto)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();

            var command = new CreateTasksCommand
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                OwnerUserId = userId
            };

            var taskId = await mediator.Send(command);
            return Ok(taskId);
        }

        // Get All Tasks (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TasksListDTO>>> GetAll([FromQuery] GetTasksListQuery query)
        {
            var result = await mediator.Send(query);
            HttpContext.InsertPaginationInformationHeader(result.TotalAMountOfRecords);
            return result.Elements;
        }

        // Get User Tasks (Users only, their own)

        [HttpGet("mytasks")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<TasksListDTO>>> GetUserTasks([FromQuery] GetMyTasksListQuery query)
        {
            var result = await mediator.Send(query);
            HttpContext.InsertPaginationInformationHeader(result.TotalAMountOfRecords);
            return result.Elements;
        }


        // Update Task (Users only, their own tasks)
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put(Guid id, UpdateTasksDTO dto)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId == null) return Unauthorized();

            var command = new UpdateTasksCommand
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                OwnerUserId = userId 
            };

            await mediator.Send(command);
            return NoContent();
        }

        // Mark Task Completed (Admin only)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, UpdateTasksStatusDTO statusDto)
        {
            var command = new UpdateTasksStatusCommand
            {
                Id = id,
                IsCompleted = statusDto.IsCompleted
            };

            await mediator.Send(command);
            return NoContent();
        }
    }
}
