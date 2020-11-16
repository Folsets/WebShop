using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebShop.IdentityServer;
using WebShop.IdentityServer.Data;
using WebShop.IdentityServer.Data.Repository;

namespace WebShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("WebShopConnection"));
            });

            services.AddScoped<IRepo, Repo>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = Constants.WEB_HOST_URL;
                    config.Audience = Constants.WebShopApiResource;
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("Admin", adminPolicy =>
                    adminPolicy.RequireClaim(JwtClaimTypes.Role, "Admin"));
                config.AddPolicy("Manager", managerPolicy =>
                    managerPolicy.RequireClaim(JwtClaimTypes.Role, "Manager"));
                config.AddPolicy("User", userPolicy =>
                    userPolicy.RequireClaim(JwtClaimTypes.Role, "User"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
