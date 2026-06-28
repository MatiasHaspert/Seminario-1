namespace Application.DTOs.Review
{
    // Reseña de una propiedad tal como se devuelve en la respuesta de la API.
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; } // Del 1 al 5
        public string Comment { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }

    }
}
