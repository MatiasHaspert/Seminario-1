
namespace Application.Services.Auth
{
    // Maneja el hash y verificación de contraseñas usando BCrypt.
    public class PasswordService
    {
        // Genera un hash seguro a partir de la contraseña en texto plano.
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Compara una contraseña en texto plano contra su hash almacenado.
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}