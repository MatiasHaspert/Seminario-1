using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Swagger
{
    /// <summary>
    /// Enriquece la documentación de los enums. Como el API los serializa como enteros,
    /// este filtro agrega a la descripción del esquema la correspondencia número → nombre
    /// (por ejemplo, <c>0 → User</c>), para que el contrato quede explícito en Swagger.
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(schema.Description))
                builder.AppendLine(schema.Description).AppendLine();

            builder.AppendLine("Valores posibles:");
            foreach (var name in Enum.GetNames(context.Type))
            {
                var value = Convert.ToInt64(Enum.Parse(context.Type, name));
                builder.AppendLine($"- `{value}` → **{name}**");
            }

            schema.Description = builder.ToString().TrimEnd();
        }
    }
}
