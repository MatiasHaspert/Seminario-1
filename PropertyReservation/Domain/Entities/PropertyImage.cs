using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // Imagen asociada a una propiedad, almacenada en el sistema de archivos del servidor.
    public class PropertyImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        public bool IsMainImage { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public int PropertyId { get; set; }

        public Property Property { get; set; } = null!;
    }
}
