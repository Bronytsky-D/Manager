using Manager.Domain.Entites;
using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.PostgreSQL.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Manager.Infrastructure.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private ManagerDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ManagerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ManagerDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                UserName = "TestUser",
                Email = "test@test.com",
                PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy"
            };

            var result = await repo.AddAsync(user);

            Assert.True(result.Success);
            Assert.NotNull(result.Result);
            Assert.Equal("TestUser", ((User)result.Result).UserName);

            var dbUser = await context.Users.FindAsync(user.Id);
            Assert.NotNull(dbUser);
            Assert.Equal("TestUser", dbUser.UserName);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            var context = GetDbContext();
            context.Users.AddRange(
                new User { UserName = "User1", Email = "u1@test.com", PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy" },
                new User { UserName = "User2", Email = "u2@test.com", PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy" }
            );
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);
            var result = await repo.GetAllAsync();

            Assert.True(result.Success);
            var users = (List<User>)result.Result;
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task FindOneAsync_Should_Return_User_If_Found()
        {
            var context = GetDbContext();
            var user = new User { UserName = "FindMe", Email = "find@test.com", PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);
            var result = await repo.FindAsync(u => u.UserName == "FindMe");

            Assert.True(result.Success);
            Assert.Equal("FindMe", ((User)result.Result).UserName);
        }

        [Fact]
        public async Task FindOneAsync_Should_Return_Failure_If_NotFound()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var result = await repo.FindAsync(u => u.UserName == "NonExisting");

            Assert.False(result.Success);
            Assert.Equal("Not found", result.Errors.First());
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            var context = GetDbContext();
            var user = new User { UserName = "OldName", Email = "old@test.com" , PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);
            user.UserName = "NewName";
            var result = await repo.UpdateAsync(user);

            Assert.True(result.Success);
            var dbUser = await context.Users.FindAsync(user.Id);
            Assert.Equal("NewName", dbUser.UserName);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_User()
        {
            var context = GetDbContext();
            var user = new User { UserName = "ToDelete", Email = "delete@test.com", PasswordHash = "$2a$11$n21Z4JnyqCdWC.RlRRqOduAyUJXlcuXHg81BUrlmwIL6jjaGgwzdy" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var repo = new UserRepository(context);
            var result = await repo.DeleteAsync(user);

            Assert.True(result.Success);
            var dbUser = await context.Users.FindAsync(user.Id);
            Assert.Null(dbUser);
        }
    }
}
