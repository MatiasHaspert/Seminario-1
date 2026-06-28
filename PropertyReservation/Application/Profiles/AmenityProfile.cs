using AutoMapper;
using Domain.Entities;
using Application.DTOs.Amenity;

namespace Application.Profiles
{
    // Mapeos entre Amenity y sus DTOs de solicitud y respuesta.
    public class AmenityProfile : Profile
    {
        public AmenityProfile()
        {
            CreateMap<Amenity, AmenityResponseDTO>();
            CreateMap<AmenityRequestDTO, Amenity>();
        }
    }
}
