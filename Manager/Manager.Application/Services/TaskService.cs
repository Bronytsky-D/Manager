using Manager.Application.Astraction.Services;
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

        public async Task<IExecutionResponse> GetAllTasksByUserIdAsync(Guid UserId)
        {
            var result = await _taskRepository.GetAllTasksByUserIdAsync(UserId);
            if (!result.Success)
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(result.Result);
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
