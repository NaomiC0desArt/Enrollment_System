using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UniversitySystem.API.Extensions
{
    public class AuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var allowAnonymous = context.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                      context.MethodInfo.DeclaringType!.IsDefined(typeof(AllowAnonymousAttribute), true);

            if (allowAnonymous) return;

            var hasAuthorize = context.MethodInfo.IsDefined(typeof(AuthorizeAttribute), true) ||
                       context.MethodInfo.DeclaringType!.IsDefined(typeof(AuthorizeAttribute), true);

            if (!hasAuthorize) return;

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                }
            };

        }
    }
}
