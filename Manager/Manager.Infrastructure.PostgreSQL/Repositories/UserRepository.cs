using Manager.Domain.Entites;
using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Manager.Infrastructure.PostgreSQL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ManagerDbContext _context;
        public UserRepository(ManagerDbContext context) { _context = context; }
        public async Task<IExecutionResponse> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity.Id);
        }

        public async Task<IExecutionResponse> GetAllAsync()
        {
            var result = await _context.Users.ToListAsync();

            return ExecutionResponse.Successful(result);
        }

        public async Task<IExecutionResponse> FindAsync(Expression<Func<User, bool>> predicate)
        {
            var result = await _context.Users.SingleOrDefaultAsync(predicate);

            return result == null
                ? ExecutionResponse.Failure("Not found")
                : ExecutionResponse.Successful(result);
        }

        public async Task<IExecutionResponse> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity.Id);
        }
    }
}
