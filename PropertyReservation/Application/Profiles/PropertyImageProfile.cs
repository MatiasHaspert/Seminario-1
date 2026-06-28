using AutoMapper;
using Domain.Entities;
using Application.DTOs.PropertyImage;

namespace Application.Profiles
{
    // Mapeo de PropertyImage hacia el DTO de respuesta.
    public class PropertyImageProfile : Profile
    {
        public PropertyImageProfile()
        {
            CreateMap<PropertyImage, PropertyImageResponseDTO>();

        }
    }
}
