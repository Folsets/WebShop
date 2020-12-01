using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace EmailService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            return services;
        }
    }
}
