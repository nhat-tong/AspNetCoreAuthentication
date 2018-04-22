using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SecureAspnetCoreApi.JWTAuthentication.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api-v1", new Info { Title = "AspnetCore API", Description = "A demo using Jwt Authentication", Version = "v1" });

                // add authorization scheme in api header
                options.AddSecurityDefinition("BearerScheme", new ApiKeyScheme
                {
                    Description = "Auhorization using Json Web Token",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "BearerScheme", new string[] { } }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/api-v1/swagger.json", "JWT Authentication API");
                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
