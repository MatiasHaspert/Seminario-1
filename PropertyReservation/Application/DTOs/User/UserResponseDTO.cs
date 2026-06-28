using Domain.Enums;

namespace Application.DTOs.User
{
    // Datos básicos del usuario devueltos en la mayoría de las respuestas de la API.
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
