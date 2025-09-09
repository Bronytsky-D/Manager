using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Infrastructure.PostgreSQL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ManagerDbContext _context;
        public Repository(ManagerDbContext context)
        {
            _context = context;
        }
        public async Task<IExecutionResponse> GetAll()
        {
            var result = await _context.Set<T>().ToListAsync();
            return ExecutionResponse.Successful(result);
        }
        public async Task<IExecutionResponse> GetById(Expression<Func<T, bool>> predicate)
        {
            var result = await _context.Set<T>().Where(predicate).ToListAsync();
            return ExecutionResponse.Successful(result);
        }
        public async Task<IExecutionResponse> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }

        public async Task<IExecutionResponse> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return ExecutionResponse.Successful(entity);
        }
    }
}
