namespace Application.DTOs.PropertyImage
{
    // Imagen de una propiedad tal como se devuelve en la respuesta de la API.
    public class PropertyImageResponseDTO
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMainImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PropertyId { get; set; }
    }
}
