using Moq;
using Manager.Application.Services;
using Manager.Doman.Entites;
using System.Linq.Expressions;
using Manager.Infrastructure.Repositories;
using Manager.Infrastructure.PostgreSQL;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _service = new UserService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateUserAsync_Should_Return_Success_When_Repository_Succeeds()
    {
        var user = new User { UserName = "test", Email = "test@mail.com" };
        _mockRepo.Setup(r => r.AddAsync(user))
                 .ReturnsAsync(ExecutionResponse.Successful(user.Id));

        var result = await _service.CreateUserAsync(user);

        Assert.True(result.Success);
        Assert.Equal(user.Id, result.Result);
        _mockRepo.Verify(r => r.AddAsync(user), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_Should_Return_Failure_When_Repository_Fails()
    {
        var user = new User { UserName = "test" };
        _mockRepo.Setup(r => r.AddAsync(user))
                 .ReturnsAsync(ExecutionResponse.Failure("Error"));

        var result = await _service.CreateUserAsync(user);

        Assert.False(result.Success);
        Assert.Equal("Error" , result.Errors.First());
        _mockRepo.Verify(r => r.AddAsync(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Call_Repository_Delete()
    {
        var user = new User { Id = Guid.NewGuid() };
        _mockRepo.Setup(r => r.DeleteAsync(user))
                 .ReturnsAsync(ExecutionResponse.Successful(user.Id));

        var result = await _service.DeleteUserAsync(user);

        Assert.True(result.Success);
        Assert.Equal(user.Id, result.Result);
        _mockRepo.Verify(r => r.DeleteAsync(user), Times.Once);
    }

    [Fact]
    public async Task GetAllUsersAsync_Should_Return_Users_From_Repository()
    {
        var users = new List<User> { new User { UserName = "u1" }, new User { UserName = "u2" } };
        _mockRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(ExecutionResponse.Successful(users));

        var result = await _service.GetAllUsersAsync();

        Assert.True(result.Success);
        Assert.Equal(2, ((List<User>)result.Result).Count);
        _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindUserAsync_Should_Return_User_When_Found()
    {
        var user = new User { UserName = "found" };
        _mockRepo.Setup(r => r.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>()))
                 .ReturnsAsync(ExecutionResponse.Successful(user));

        var result = await _service.FindUserAsync(u => u.UserName == "found");

        Assert.True(result.Success);
        Assert.Equal(user, result.Result);
        _mockRepo.Verify(r => r.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_Should_Return_Updated_User()
    {
        var user = new User { UserName = "update" };
        _mockRepo.Setup(r => r.UpdateAsync(user))
                 .ReturnsAsync(ExecutionResponse.Successful(user));

        var result = await _service.UpdateUserAsync(user);

        Assert.True(result.Success);
        Assert.Equal(user, result.Result);
        _mockRepo.Verify(r => r.UpdateAsync(user), Times.Once);
    }
}
