using Manager.Domain.Entites;
using Manager.Infrastructure;
using System.Linq.Expressions;

namespace Manager.Application.Astraction.Services
{
    public interface IUserService
    {
        Task<IExecutionResponse> GetAllUsersAsync();
        Task<IExecutionResponse> FindUserAsync(Expression<Func<User, bool>> predicate);
        Task<IExecutionResponse> CreateUserAsync(User user);
        Task<IExecutionResponse> UpdateUserAsync(User user);
        Task<IExecutionResponse> DeleteUserAsync(User user);
    }
}
