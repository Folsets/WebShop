using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using IdentityModel;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using WebShop.Api.Options;
using WebShop.Data;
using Constants = WebShop.IdentityServer.Constants;


namespace WebShop.Api

{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataRepositories();

            services.AddCors();

            services.AddControllers();

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

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Protected WebShop API",
                    Version = "v1"
                });

                x.OperationFilter<AuthorizeCheckOperationFilter>();

                x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Constants.WEB_HOST_URL}connect/authorize"),
                            TokenUrl = new Uri($"{Constants.WEB_HOST_URL}connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {Constants.WebShopApiResource, "API - full access"}
                            },
                        }
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);

                options.OAuthClientId("api_swagger");
                options.OAuthAppName("Swagger UI for API");
                options.OAuthUsePkce();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(
                options =>
                {
                    options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                });
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
