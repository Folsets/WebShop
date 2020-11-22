using Microsoft.Extensions.DependencyInjection;
using WebShop.Data.Interfaces;
using WebShop.Data.Repos;

namespace WebShop.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddTransient<IClientRepository, ClientRepository>();
            // More to add...

            return services;
        }
    }
}
