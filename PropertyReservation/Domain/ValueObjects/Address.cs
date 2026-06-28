using System.ComponentModel.DataAnnotations;

namespace Domain.ValueObjects
{
    // Dirección física de una propiedad o usuario, almacenada como value object dentro de la entidad.
    public class Address
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        public Address(string country, string state, string city, int postalCode, string streetAddress)
        {
            Country = country;
            State = state;
            City = city;
            PostalCode = postalCode;
            StreetAddress = streetAddress;
        }

    }
}
