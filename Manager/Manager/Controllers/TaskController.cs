using Manager.Application.Astraction.Services;
using Manager.Common.DTOs;
using Manager.Infrastructure.PostgreSQL;
using Manager.Domain.Entites;
using Manager.Domain.Enums;
using Manager.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Manager.Controllers
{
    [Authorize]
    [Route("tasks")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IExecutionResponse> CreateTask(CreateTaskRequestDto reques)  
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return ExecutionResponse.Failure("User not authorized.");
            
            Guid userIdGuid = Guid.Parse(userIdClaim);

            var task = new TaskEntity
            {
                UserId = userIdGuid,
                Title = reques.Title,
                Description = reques.Description,
                DueDate = reques.DueDate,
                Status = reques.Status ?? Status.Pending,
                Priority = reques.Prioritys ?? Prioritys.Medium
            };  

            var respone = await _taskService.CreateTaskAsync(task);

            return respone;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] TaskFilterRequestDto filters)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            Guid userIdGuid = Guid.Parse(userIdClaim);

            var respone = await _taskService.GetAllTasksByUserIdAsync(userIdGuid, filters);
            return Ok(respone);
        }
    
        [HttpGet("{id:guid}")]
        public async Task<IExecutionResponse> GetTaskById(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return ExecutionResponse.Failure("User not authorized.");

            Guid userId = Guid.Parse(userIdClaim);

            var respone = await _taskService.FindTaskAsync(t => t.Id == id && t.UserId == userId);
            return respone;
        }
        [HttpDelete("{id:guid}")]
        public async Task<IExecutionResponse> DeleteTask(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return ExecutionResponse.Failure("User not authorized.");

            Guid userId = Guid.Parse(userIdClaim);

            var taskResult = await _taskService.FindTaskAsync(t => t.Id == id && t.UserId == userId);
            if (!taskResult.Success)
                return ExecutionResponse.Failure("Not exist task"); 
            var task = (TaskEntity)taskResult.Result;

            var respone = await _taskService.DeleteTaskAsync(task);

            return respone;
        }
        [HttpPut("{id:guid}")]
        public async Task<IExecutionResponse> UpdateTask(Guid id, [FromBody] UpdateTaskRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return ExecutionResponse.Failure("User not authenticated");

            Guid userId = Guid.Parse(userIdClaim.Value);

            var taskResult = await _taskService.FindTaskAsync(t => t.Id == id && t.UserId == userId);
            if (!taskResult.Success || taskResult.Result == null)
                return ExecutionResponse.Failure("Task not found");

            var task = (TaskEntity)taskResult.Result;

            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.DueDate = request.DueDate ?? task.DueDate;
            task.Status = request.Status ?? task.Status;
            task.Priority = request.Priority ?? task.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            var updateResult = await _taskService.UpdateTaskAsync(task.Id, task);
            if (!updateResult.Success)
                return ExecutionResponse.Failure(updateResult.Errors);

            return ExecutionResponse.Successful(task.Id);
        }
    }
}
