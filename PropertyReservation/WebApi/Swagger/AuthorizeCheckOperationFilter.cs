using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Swagger
{
    /// <summary>
    /// Documenta los requisitos de seguridad endpoint por endpoint. En lugar de aplicar
    /// el candado de autenticación de forma global, lo agrega únicamente a las operaciones
    /// protegidas (con <see cref="AuthorizeAttribute"/> y sin <see cref="AllowAnonymousAttribute"/>),
    /// junto con las respuestas estándar 401 y 403. Así los endpoints públicos quedan limpios.
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodAttributes = context.MethodInfo.GetCustomAttributes(true);
            var classAttributes = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                                   ?? Array.Empty<object>();

            // El [AllowAnonymous] a nivel de método tiene prioridad y deja el endpoint público.
            if (methodAttributes.OfType<AllowAnonymousAttribute>().Any())
                return;

            // El endpoint está protegido si hay [Authorize] en el método o en el controlador.
            var authorizeAttributes = methodAttributes.OfType<AuthorizeAttribute>()
                .Concat(classAttributes.OfType<AuthorizeAttribute>())
                .ToList();

            if (authorizeAttributes.Count == 0)
                return;

            // Respuestas de seguridad estándar para toda operación protegida.
            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "No autenticado: falta el token JWT o no es válido."
            });
            operation.Responses.TryAdd("403", new OpenApiResponse
            {
                Description = "Autenticado, pero el rol del usuario no tiene permiso para este recurso."
            });

            // Exige el esquema Bearer solo en esta operación (muestra el candado en Swagger UI).
            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [bearerScheme] = Array.Empty<string>()
            });

            // Si el endpoint exige roles concretos, los anexa a la descripción para mayor claridad.
            var roles = authorizeAttributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                .SelectMany(a => a.Roles!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .Distinct()
                .ToList();

            if (roles.Count > 0)
            {
                var rolesText = $"🔒 **Roles permitidos:** {string.Join(", ", roles)}.";
                operation.Description = string.IsNullOrWhiteSpace(operation.Description)
                    ? rolesText
                    : $"{operation.Description}\n\n{rolesText}";
            }
        }
    }
}
