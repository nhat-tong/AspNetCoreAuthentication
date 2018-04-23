using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace SecureAspnetCoreApi.JWTAuthentication.Framework
{
    public class AddVersionHeader : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var version = swaggerDoc.Info.Version;

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                TryAddVersionParamTo(pathItem.Get, version);
                TryAddVersionParamTo(pathItem.Post, version);
                TryAddVersionParamTo(pathItem.Put, version);
                TryAddVersionParamTo(pathItem.Delete, version);
            }
        }

        private void TryAddVersionParamTo(Operation operation, string version)
        {
            if (operation == null) return;

            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "v",
                In = "header",
                Type = "string",
                Default = version.Replace("v", "")
            });
        }
    }
}
