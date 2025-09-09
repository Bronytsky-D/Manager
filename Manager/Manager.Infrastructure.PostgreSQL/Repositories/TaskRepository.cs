using Manager.Doman.Entites;
using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Manager.Infrastructure.PostgreSQL.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly ManagerDbContext _context;
        public TaskRepository(ManagerDbContext context) { _context = context; }

        public Task<IExecutionResponse> Add(TaskEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IExecutionResponse> Delete(TaskEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IExecutionResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IExecutionResponse> GetById(Expression<Func<TaskEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IExecutionResponse> Update(TaskEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
