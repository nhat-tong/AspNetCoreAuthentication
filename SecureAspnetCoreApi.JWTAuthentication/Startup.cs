#region using
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureAspnetCoreApi.JWTAuthentication.Configuration;
#endregion

namespace SecureAspnetCoreApi.JWTAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _env;

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
            // Configure https for staging/production: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.0&tabs=visual-studio 
            if (!_env.IsDevelopment())
            {
                services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));
            }

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
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
            }

            app.UseCustomSwagger();

            // Enable JWT Auth Service
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
