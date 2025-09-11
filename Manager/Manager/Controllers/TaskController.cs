using Manager.Application.Astraction.Services;
using Manager.Common.DTOs;
using Manager.Infrastructure.PostgreSQL;
using Manager.Doman.Entites;
using Manager.Doman.Enums;
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
                return ExecutionResponse.Failure("");
            
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
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            Guid userId = Guid.Parse(userIdClaim);

            var respone = await _taskService.FindTaskAsync(t => t.Id == id && t.UserId == userId);
            return Ok();
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
        //[HttpPut("{id:guid}")]
        //public async Task<IExecutionResponse> CangeTask(Guid id, [FromBody] UpdateTaskRequestDto request)
        //{
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //        return ExecutionResponse.Failure("Not exist user");

        //    Guid userId = Guid.Parse(userIdClaim.Value);

        //    var taskResult = await _taskService.FindTaskAsync(t => t.Id == id && t.UserId == userId);
        //    var task = (TaskEntity)taskResult.Result;

        //    var task = new TaskEntity
        //    {
        //        Id =
        //    }
        //    return new ExecutionResponse()
        //}
    }
}
