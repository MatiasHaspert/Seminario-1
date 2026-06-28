namespace Application.DTOs.User
{
    // Vista detallada de un usuario con sus estadísticas, para uso del administrador.
    public class UserDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        // Cantidad de reservas y propiedades del usuario
        public int ReservationsCount { get; set; }
        public int PropertiesCount { get; set; }
    }
}
