using Manager.Doman.Entites;
using Manager.Infrastructure;

namespace Manager.Application.Astraction.Services
{
    public interface ITaskService
    {
        Task<IExecutionResponse> GetAllTasksAsync();
        Task<IExecutionResponse> GetTaskByIdAsync(Guid id);
        Task<IExecutionResponse> CreateTaskAsync(TaskEntity task);
        Task<IExecutionResponse> UpdateTaskAsync(TaskEntity task);
        Task<IExecutionResponse> DeleteTaskAsync(TaskEntity task);
    }
}
