using AutoMapper;
using Domain.Entities;
using Application.DTOs.PropertyAvailability;

namespace Application.Profiles
{
    // Mapeos entre PropertyAvailability y sus distintos DTOs (respuesta, solicitud y público).
    public class PropertyAvailabilityProfile : Profile
    {
        public PropertyAvailabilityProfile()
        {
            CreateMap<PropertyAvailability, PropertyAvailabilityResponseDTO>();
            CreateMap<PropertyAvailabilityRequestDTO, PropertyAvailability>();
            CreateMap<PropertyAvailability, PropertyAvailabilityPublicDTO>();
        }
    }
}
