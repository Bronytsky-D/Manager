using Manager.Domain.Entites;
using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Manager.Infrastructure.PostgreSQL.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly ManagerDbContext _context;
        public TaskRepository(ManagerDbContext context) { _context = context; }
        public async Task<IExecutionResponse> GetAllTasksByUserIdAsync(Guid userId)
        {
            var result = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
            return ExecutionResponse.Successful(result);
        }
        public async Task<IExecutionResponse> GetAllAsync()
        {
            var result = await _context.Tasks.ToListAsync();
            return ExecutionResponse.Successful(result);
        }

        public async Task<IExecutionResponse> AddAsync(TaskEntity entity)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> DeleteAsync(TaskEntity entity)
        {
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity.Id);
        }

        public async Task<IExecutionResponse> GetAllByUserIdAsync()
        {
            var result = await _context.Tasks.ToListAsync();

            return ExecutionResponse.Successful(result);
        }

        public async Task<IExecutionResponse> FindAsync(Expression<Func<TaskEntity, bool>> predicate)
        {
            var result = await _context.Tasks.SingleOrDefaultAsync(predicate);
            
            return ExecutionResponse.Successful(result);
        }

        public async Task<IExecutionResponse> UpdateAsync(TaskEntity entity)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
        
            return ExecutionResponse.Successful(entity.Id);
        }
    }
}
