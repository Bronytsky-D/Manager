using Manager.Application.Astraction.Services;
using Manager.Doman.Entites;
using Manager.Infrastructure;
using Manager.Infrastructure.PostgreSQL;
using Manager.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Manager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<IExecutionResponse> CreateUserAsync(User user)
        {
            var result = await _userRepository.AddAsync(user);
            if(!result.Success) 
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(user.Id);
        }

        public async Task<IExecutionResponse> DeleteUserAsync(User user)
        {
            var result = await _userRepository.DeleteAsync(user);
            if(!result.Success) 
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(user.Id);
        }

        public async Task<IExecutionResponse> GetAllUsersAsync()
        {
            var result = await _userRepository.GetAllAsync();
            if(!result.Success) 
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(result.Result);
        }

        public async Task<IExecutionResponse> FindUserAsync(Expression<Func<User, bool>> predicate)
        {
            var result = await _userRepository.FindAsync(predicate);
            if(!result.Success) 
                return ExecutionResponse.Failure(result.Errors);
            
            return ExecutionResponse.Successful(result.Result);
        }

        public async Task<IExecutionResponse> UpdateUserAsync(User user)
        {
            var result = await _userRepository.UpdateAsync(user);
            if(!result.Success) 
                return ExecutionResponse.Failure(result.Errors);

            return ExecutionResponse.Successful(result.Result);
        }
    }
}
