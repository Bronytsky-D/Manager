using Manager.Common.DTOs;
using Manager.Doman.Entites;
using Manager.Infrastructure;
using System.Linq.Expressions;

namespace Manager.Application.Astraction.Services
{
    public interface ITaskService
    {
        Task<IExecutionResponse> GetAllTasksByUserIdAsync(Guid userId, TaskFilterRequestDto filters);
        Task<IExecutionResponse> FindTaskAsync(Expression<Func<TaskEntity, bool>> predicate);
        Task<IExecutionResponse> CreateTaskAsync(TaskEntity task);
        Task<IExecutionResponse> UpdateTaskAsync(Guid id ,TaskEntity task);
        Task<IExecutionResponse> DeleteTaskAsync(TaskEntity task);
    }
}
