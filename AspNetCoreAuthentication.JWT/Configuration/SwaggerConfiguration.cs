using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreAuthentication.JWT.Framework;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AspNetCoreAuthentication.JWT.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("api-v1", new Info { Title = "Jwt Auth API v1", Description = "A demo using Jwt Authentication", Version = "v1" });
                options.SwaggerDoc("api-v2", new Info { Title = "Jwt Auth API v2", Description = "A demo using Jwt Authentication", Version = "v2" });

                options.CustomSchemaIds(x => x.FullName);

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

                // Create Swagger Documents by version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"api-v{v.MajorVersion}" == docName);
                });

                // Add custom version header
                options.DocumentFilter<AddVersionHeader>();
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/api-v1/swagger.json", "JWT Authentication API v1");
                options.SwaggerEndpoint("/swagger/api-v2/swagger.json", "JWT Authentication API v2");

                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
