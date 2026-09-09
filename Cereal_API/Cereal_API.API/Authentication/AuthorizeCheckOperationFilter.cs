using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cereal_API.API.Authentication;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool hasAuthorize =
        context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
        (context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>().Any() ?? false);

        if (!hasAuthorize)
            return;

        operation.Security ??= [];

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Basic",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}
