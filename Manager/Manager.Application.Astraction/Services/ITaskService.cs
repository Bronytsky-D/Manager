using Manager.Doman.Entites;
using Manager.Infrastructure;
using System.Linq.Expressions;

namespace Manager.Application.Astraction.Services
{
    public interface ITaskService
    {
        Task<IExecutionResponse> GetAllTasksAsync();
        Task<IExecutionResponse> FindTaskAsync(Expression<Func<TaskEntity, bool>> predicate);
        Task<IExecutionResponse> CreateTaskAsync(TaskEntity task);
        Task<IExecutionResponse> UpdateTaskAsync(TaskEntity task);
        Task<IExecutionResponse> DeleteTaskAsync(TaskEntity task);
    }
}
