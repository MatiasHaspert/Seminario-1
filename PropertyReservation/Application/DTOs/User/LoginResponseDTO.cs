namespace Application.DTOs.User
{
    // Respuesta al iniciar sesión: incluye el token JWT y los datos básicos del usuario.
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseDTO User { get; set; } = null!;
    }
}
