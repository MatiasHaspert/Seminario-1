using Application.Profiles;
using Application.Services;
using Application.Services.Auth;
using Infrastructure.Context;
using Infrastructure.DemoData;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Security.Claims;
using System.Text;
using WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
    );


// Registro de repositorios
builder.Services.AddScoped<PropertyRepository>();
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<AmenityRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<PropertyAvailabilityRepository>();
builder.Services.AddScoped<PropertyImageRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PaymentsRepository>();
builder.Services.AddScoped<AdminStatsRepository>();

// Registro de servicios de aplicación
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<PropertyImageService>();
builder.Services.AddScoped<PropertyAvailabilityService>();
builder.Services.AddScoped<AmenityService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<JwtTokenGeneratorService>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(PropertyProfile));
builder.Services.AddScoped<PaymentsService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUserService>();

builder.Services.AddScoped<UserManagementService>();
builder.Services.AddScoped<AdminStatsService>();

// Documentación OpenAPI/Swagger (configuración centralizada en WebApi.Swagger).
builder.Services.AddSwaggerDocumentation();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy
            .WithOrigins("http://localhost:5173") // URL del frontend en desarrollo
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Parámetros de validación del token JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuer = true,             // Verificar que el token provenga de nuestro servidor
    ValidateAudience = false,          // No validamos audiencia por ahora
    ValidateLifetime = true,           // Rechazar tokens expirados
    ValidateIssuerSigningKey = true,   // Verificar que la firma sea correcta

    ValidIssuer = jwtSettings["Issuer"],
    // ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key),

    NameClaimType = ClaimTypes.NameIdentifier,
    RoleClaimType = ClaimTypes.Role,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // El middleware de ASP.NET Core lee el header "Authorization: Bearer <token>",
    // valida el token contra estos parámetros y puebla HttpContext.User automáticamente.
    options.TokenValidationParameters = tokenValidationParams;

    // El token usa nombres de claim cortos (sub, role, unique_name). Mapearlos a los
    // tipos largos (ClaimTypes.NameIdentifier / ClaimTypes.Role) para que coincidan con
    // NameClaimType/RoleClaimType, IsInRole(...) y CurrentUserService.
    options.MapInboundClaims = true;
});


// Política de autorización exclusiva para el rol Admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Property Reservation System API v1");
        options.DocumentTitle = "Property Reservation System — API Docs";
        options.DocExpansion(DocExpansion.List);     // Lista los endpoints contraídos, agrupados por tag
        options.DefaultModelsExpandDepth(1);         // Muestra los esquemas de modelos al final
        options.DisplayRequestDuration();            // Muestra cuánto tardó cada request de prueba
        options.EnableTryItOutByDefault();           // Habilita "Try it out" sin un clic extra
        options.EnablePersistAuthorization();        // Conserva el token JWT al recargar la página
    });
}

app.UseCors("AllowReact");

app.UseStaticFiles(); // Debe ir antes de MapControllers para servir los archivos de wwwroot

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DemoDataSeeder.SeedAsync(db);
}

app.Run();
