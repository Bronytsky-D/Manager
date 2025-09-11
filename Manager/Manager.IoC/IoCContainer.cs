using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Manager.IoC.Conteiners;

namespace Manager.IoC
{
    public static class IoCContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configurationManager)
        {
            InfrasrtuctureContainer.Register(services, configurationManager);
            ApplicationContainer.Register(services, configurationManager);
        }

    }
}
