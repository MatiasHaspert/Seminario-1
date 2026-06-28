using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Swagger
{
    /// <summary>
    /// Configuración centralizada de la documentación OpenAPI/Swagger del API.
    /// Mantener todo aquí deja el <c>Program.cs</c> limpio y declarativo.
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Property Reservation System API",
                    Version = "v1",
                    Description = """
                        API REST del **Sistema de Reservas de Propiedades**.

                        Permite gestionar propiedades, disponibilidad, reservas, pagos, reseñas,
                        servicios (*amenities*) y usuarios. La autenticación es por **JWT** y la
                        autorización por roles: `User` (huésped), `Owner` (dueño) y `Admin`.

                        ### Cómo autenticarse
                        1. Obtené un token con `POST /api/Auth/login`.
                        2. Hacé clic en **Authorize** 🔒 (arriba a la derecha) e ingresá `Bearer {token}`.
                        3. A partir de ahí, los endpoints protegidos enviarán el token automáticamente.

                        ### Usuarios de demostración
                        | Rol            | Email              | Contraseña |
                        |----------------|--------------------|------------|
                        | Admin          | admin@example.com  | admin      |
                        | Owner (dueño)  | owner1@example.com | owner1     |
                        | User (huésped) | user1@example.com  | user1      |
                        """,
                    Contact = new OpenApiContact
                    {
                        Name = "Equipo Property Reservation System",
                        Email = "soporte@propertyreservation.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Uso académico — AUS Seminario I"
                    }
                });

                // Definición del esquema de seguridad JWT Bearer.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Autenticación JWT. Pegá únicamente el token (Swagger antepone «Bearer » por vos)."
                });

                // El requisito de seguridad se aplica por operación, no global (ver filtro).
                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.SchemaFilter<EnumSchemaFilter>();

                // Respeta los tipos no anulables de C# al describir los esquemas.
                options.SupportNonNullableReferenceTypes();
                options.UseAllOfToExtendReferenceSchemas();

                // Ordena las operaciones por ruta y método para una lectura prolija.
                options.OrderActionsBy(api => $"{api.RelativePath}_{api.HttpMethod}");

                // Comentarios XML del API (incluye los <summary> de los controladores como
                // descripción de cada tag) y de la capa de aplicación (DTOs).
                IncludeXmlComments(options, "WebApi.xml", includeControllerComments: true);
                IncludeXmlComments(options, "Application.xml");
            });

            return services;
        }

        private static void IncludeXmlComments(SwaggerGenOptions options, string fileName, bool includeControllerComments = false)
        {
            var path = Path.Combine(AppContext.BaseDirectory, fileName);
            if (File.Exists(path))
                options.IncludeXmlComments(path, includeControllerXmlComments: includeControllerComments);
        }
    }
}
