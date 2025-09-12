using Manager.Doman.Entites;
using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infrastructure.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly TaskRepository _repository;
        private readonly ManagerDbContext _context;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ManagerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ManagerDbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Task()
        {
            var task = new TaskEntity { Title = "Test Task", UserId = Guid.NewGuid() };

            var result = await _repository.AddAsync(task);

            Assert.True(result.Success);
            var savedTask = await _context.Tasks.FindAsync(task.Id);
            Assert.NotNull(savedTask);
            Assert.Equal("Test Task", savedTask.Title);
        }

        [Fact]
        public async Task GetAllTasksByUserIdAsync_Should_Return_Only_User_Tasks()
        {
            var userId = Guid.NewGuid();
            var task1 = new TaskEntity { Title = "Task1", UserId = userId };
            var task2 = new TaskEntity { Title = "Task2", UserId = Guid.NewGuid() };

            await _context.Tasks.AddRangeAsync(task1, task2);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllTasksByUserIdAsync(userId);

            Assert.True(result.Success);
            var tasks = (List<TaskEntity>)result.Result;
            Assert.Single(tasks);
            Assert.Equal("Task1", tasks[0].Title);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Task()
        {
            var task = new TaskEntity { Title = "To Delete", UserId = Guid.NewGuid() };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var result = await _repository.DeleteAsync(task);

            Assert.True(result.Success);
            var deleted = await _context.Tasks.FindAsync(task.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Task()
        {
            var task = new TaskEntity { Title = "Old Title", UserId = Guid.NewGuid() };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated Title";
            var result = await _repository.UpdateAsync(task);

            Assert.True(result.Success);
            var updated = await _context.Tasks.FindAsync(task.Id);
            Assert.Equal("Updated Title", updated.Title);
        }

        [Fact]
        public async Task FindOneAsync_Should_Return_Task_When_Exists()
        {
            var task = new TaskEntity { Title = "Find Me", UserId = Guid.NewGuid() };
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            var result = await _repository.FindAsync(t => t.Title == "Find Me");

            Assert.True(result.Success);
            var found = (TaskEntity)result.Result;
            Assert.Equal(task.Id, found.Id);
        }
    }
}
