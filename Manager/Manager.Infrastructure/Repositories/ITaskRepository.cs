using Manager.Doman.Entites;
using System.Linq.Expressions;

namespace Manager.Infrastructure.Repositories
{
    public interface ITaskRepository: IRepository<TaskEntity>
    {
        Task<IExecutionResponse> FindOneAsync(Expression<Func<TaskEntity, bool>> predicate);
    }
}
