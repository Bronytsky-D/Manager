
using Manager.Doman.Entites;
using System.Linq.Expressions;

namespace Manager.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IExecutionResponse> GetAllAsync();
        Task<IExecutionResponse> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IExecutionResponse> AddAsync(T entity);
        Task<IExecutionResponse> UpdateAsync(T entity);
        Task<IExecutionResponse> DeleteAsync(T entity);
    }
}
