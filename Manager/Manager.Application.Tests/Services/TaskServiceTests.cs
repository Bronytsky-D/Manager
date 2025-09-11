using Manager.Application.Services;
using Manager.Common.DTOs;
using Manager.Doman.Entites;
using Manager.Doman.Enums;
using Manager.Infrastructure.PostgreSQL;
using Manager.Infrastructure.Repositories;
using Moq;
using System.Linq.Expressions;


public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _service = new TaskService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_Should_Return_Success_When_Repository_Succeeds()
    {
        var task = new TaskEntity { Title = "Task 1" };
        _mockRepo.Setup(r => r.AddAsync(task))
                 .ReturnsAsync(ExecutionResponse.Successful(task.Id));

        var result = await _service.CreateTaskAsync(task);

        Assert.True(result.Success);
        Assert.Equal(task.Id, result.Result);
        _mockRepo.Verify(r => r.AddAsync(task), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_Should_Return_Failure_When_Repository_Fails()
    {
        var task = new TaskEntity { Title = "Task 1" };
        _mockRepo.Setup(r => r.AddAsync(task))
                 .ReturnsAsync(ExecutionResponse.Failure(new[] { "Error" }));

        var result = await _service.CreateTaskAsync(task);

        Assert.False(result.Success);
        Assert.Equal("Error", result.Errors.First());
        _mockRepo.Verify(r => r.AddAsync(task), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_Should_Return_Success_When_Repository_Succeeds()
    {
        var task = new TaskEntity { Id = Guid.NewGuid() };
        _mockRepo.Setup(r => r.DeleteAsync(task))
                 .ReturnsAsync(ExecutionResponse.Successful(task.Id));

        var result = await _service.DeleteTaskAsync(task);

        Assert.True(result.Success);
        Assert.Equal(task.Id, result.Result);
        _mockRepo.Verify(r => r.DeleteAsync(task), Times.Once);
    }

    [Fact]
    public async Task GetAllTasksByUserIdAsync_Should_Filter_And_Paginate()
    {
        var tasks = new List<TaskEntity>
    {
        new TaskEntity { Title = "T1", Status = Status.Pending, Priority = Prioritys.Low, DueDate = DateTime.Today },
        new TaskEntity { Title = "T2", Status = Status.Completed, Priority = Prioritys.High, DueDate = DateTime.Today.AddDays(1) },
        new TaskEntity { Title = "T3", Status = Status.Pending, Priority = Prioritys.Medium, DueDate = DateTime.Today }
    };

        _mockRepo.Setup(r => r.GetAllTasksByUserIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync(ExecutionResponse.Successful(tasks));

        var filters = new TaskFilterRequestDto
        {
            Status = "Pending",
            Priority = "Medium",
            Page = 1,
            PageSize = 10,
            SortBy = "duedate",
            Desc = false,
            DueDate = DateTime.Today
        };

        var result = await _service.GetAllTasksByUserIdAsync(Guid.NewGuid(), filters);

        Assert.True(result.Success);

        var pagedResult = (GetTasksPagedResultDto)result.Result;

        Assert.Equal(1, pagedResult.TotalCount);
        Assert.Single(pagedResult.Items);
        Assert.Equal("T3", pagedResult.Items[0].Title);
    }

    [Fact]
    public async Task FindTaskAsync_Should_Return_Task_When_Found()
    {
        var task = new TaskEntity { Title = "FoundTask" };
        _mockRepo.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<TaskEntity, bool>>>()))
                 .ReturnsAsync(ExecutionResponse.Successful(task));

        var result = await _service.FindTaskAsync(t => t.Title == "FoundTask");

        Assert.True(result.Success);
        Assert.Equal(task, result.Result);
    }

    [Fact]
    public async Task UpdateTaskAsync_Should_Return_Success_When_Repository_Succeeds()
    {
        var task = new TaskEntity { Id = Guid.NewGuid(), Title = "Old" };
        _mockRepo.Setup(r => r.FindOneAsync(t => t.Id == task.Id))
                 .ReturnsAsync(ExecutionResponse.Successful(task));
        _mockRepo.Setup(r => r.UpdateAsync(task))
                 .ReturnsAsync(ExecutionResponse.Successful(task));

        var result = await _service.UpdateTaskAsync(task.Id, task);

        Assert.True(result.Success);
        Assert.Equal(task.Id, result.Result);
        _mockRepo.Verify(r => r.UpdateAsync(task), Times.Once);
    }
}
