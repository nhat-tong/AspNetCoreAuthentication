using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureAspnetCoreApi.JWTAuthentication.Configuration;

namespace SecureAspnetCoreApi.JWTAuthentication
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
            // Configure JWT Auth Service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddCustomJwtBearer(Configuration["Jwt:Issuer"], 
                                        Configuration["Jwt:Audience"], 
                                        Configuration["Jwt:Key"]);

            // Configure API Versioning
            services.AddApiVersioning(options => 
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                //options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("v"), 
                                                                    new HeaderApiVersionReader() { HeaderNames = { "v" } });
                options.ReportApiVersions = true;
            });

            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AgeRestriction", (policy) =>
                {
                    policy.Requirements.Add(new Framework.AgeRestrictionRequirement(18));
                    // Issue 1726: https://github.com/aspnet/Security/issues/1726
                    policy.RequireAuthenticatedUser();
                });
            });
            services.AddSingleton<IAuthorizationHandler, Framework.AgeRestrictionHandler>();

            // Register Swagger Generator
            services.AddCustomSwaggerGen();

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomSwagger();

            // Enable JWT Auth Service
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
