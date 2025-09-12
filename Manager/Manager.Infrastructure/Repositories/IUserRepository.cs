
using Manager.Doman.Entites;
using System.Linq.Expressions;

namespace Manager.Infrastructure.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        Task<IExecutionResponse> FindAsync(Expression<Func<User, bool>> predicate);
    }
}
