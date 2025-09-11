using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infrastructure.PostgreSQL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ManagerDbContext _context;
        public Repository(ManagerDbContext context)
        {
            _context = context;
        }
        public async Task<IExecutionResponse> GetAllAsync()
        {
            var result = await _context.Set<T>().ToListAsync();
            return ExecutionResponse.Successful(result);
        }
        public async Task<IExecutionResponse> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }
    }
}
