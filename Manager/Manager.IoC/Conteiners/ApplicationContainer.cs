using Manager.Application.Astraction.Services;
using Manager.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.IoC.Conteiners
{
    public static class ApplicationContainer
    {
        public static void Register(IServiceCollection services, IConfiguration configurationManager)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();

            //
        }
    }
}
