using Manager.Infrastructure.PostgreSQL.DbContex;
using Manager.Infrastructure.PostgreSQL.Repositories;
using Manager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Manager.IoC.Conteiners
{
    public static class InfrasrtuctureContainer
    {
        public static void Register(this IServiceCollection services, IConfiguration configurationManager)
        {
            services.AddDbContext<ManagerDbContext>(option =>
                option.UseNpgsql(configurationManager.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

    }
}
