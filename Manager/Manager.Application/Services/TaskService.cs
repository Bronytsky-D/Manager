using Manager.Application.Astraction.Services;
using Manager.Common.DTOs;
using Manager.Doman.Entites;
using Manager.Infrastructure;
using Manager.Infrastructure.PostgreSQL;
using Manager.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Manager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository) 
        {
            _taskRepository = taskRepository;
        }
        public async Task<IExecutionResponse> CreateTaskAsync(TaskEntity task)
        {
            var result = await _taskRepository.AddAsync(task);
            if(!result.Success)
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(task.Id);
        }

        public async Task<IExecutionResponse> DeleteTaskAsync(TaskEntity task)
        {
            var result = await _taskRepository.DeleteAsync(task);
            if (!result.Success)
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(task.Id);
        }

        public async Task<IExecutionResponse> GetAllTasksByUserIdAsync(Guid userId, TaskFilterRequestDto filters)
        {
            var allTasksResult = await _taskRepository.GetAllTasksByUserIdAsync(userId);
            var allTasks = (List<TaskEntity>)allTasksResult.Result;
            var query = allTasks.AsQueryable();

            var status = filters.GetStatus();
            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            var priority = filters.GetPriority();
            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (filters.DueDate.HasValue)
            {
                var date = filters.DueDate.Value.Date;
                var next = date.AddDays(1);

                query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value >= date && t.DueDate.Value < next);
            }

            if (!string.IsNullOrEmpty(filters.SortBy))
            {
                query = filters.SortBy.ToLower() switch
                {
                    "duedate" => filters.Desc
                        ? query.OrderByDescending(t => t.DueDate)
                        : query.OrderBy(t => t.DueDate),

                    "priority" => filters.Desc
                        ? query.OrderByDescending(t => t.Priority)
                        : query.OrderBy(t => t.Priority),

                    _ => query
                };
            }

            
            var totalCount = query.Count();
            var items = query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList();

            return ExecutionResponse.Successful(new
            {
                TotalCount = totalCount,
                Page = filters.Page,
                PageSize = filters.PageSize,
                Items = items
            });
        }

        public async Task<IExecutionResponse> FindTaskAsync(Expression<Func<TaskEntity, bool>> predicate)
        {
            var result = await _taskRepository.FindOneAsync(predicate);
            if (!result.Success)
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(result.Result);
        }

        public async Task<IExecutionResponse> UpdateTaskAsync(Guid taskId, TaskEntity task)
        {
            var taskResult = await _taskRepository.FindOneAsync(t => t.Id == taskId);

            var result = await _taskRepository.UpdateAsync(task);
            if (!result.Success)
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(task.Id);
        }
    }
}
