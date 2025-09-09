
using System.Linq.Expressions;

namespace Manager.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IExecutionResponse> GetAll();
        Task<IExecutionResponse> GetById(Expression<Func<T, bool>> predicate);
        Task<IExecutionResponse> Add(T entity);
        Task<IExecutionResponse> Update(T entity);
        Task<IExecutionResponse> Delete(T entity);
    }
}
