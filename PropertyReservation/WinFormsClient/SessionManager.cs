using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WinFormsClient;

// Gestor de sesión para mantener el estado de autenticación del usuario.
public static class SessionManager
{
    private static string? _jwtToken;
    private static List<Claim> _claims = new();

    // Obtiene o establece el token JWT. Al establecer un nuevo token,
    // se decodifica automáticamente para extraer y almacenar los claims del usuario.
    public static string? JwtToken
    {
        get => _jwtToken;
        set
        {
            _jwtToken = value;
            _claims.Clear(); // Limpiar claims anteriores al establecer un nuevo token.

            if (!string.IsNullOrEmpty(_jwtToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(_jwtToken); // Decodificar el token para extraer los claims publicos.
                _claims.AddRange(token.Claims);
            }
        }
    }

    // Indica si el usuario está actualmente autenticado.
    public static bool IsUserAuthenticated => !string.IsNullOrEmpty(JwtToken);

    // Obtiene el rol del usuario autenticado desde los claims del token.
    public static string? GetUserRole() =>
        _claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

    // Obtiene el ID del usuario autenticado desde los claims del token.
    public static string? GetUserId() =>
        _claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;

    // Obtiene el email del usuario autenticado desde los claims del token.
    public static string? GetUserEmail() =>
        _claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;

    // Cierra la sesión del usuario eliminando el token y los claims.
    public static void Logout()
    {
        _jwtToken = null;
        _claims.Clear();
    }
}
