using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // Reseña que un huésped escribe sobre una propiedad después de completar su estadía.
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Rating { get; set; } // Del 1 al 5

        public string Comment { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public int PropertyId { get; set; }

        public Property Property { get; set; } = null!;

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
