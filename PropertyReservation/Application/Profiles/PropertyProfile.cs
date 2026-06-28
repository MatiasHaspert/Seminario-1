using AutoMapper;
using Application.DTOs.Property;
using Domain.Entities;
using Domain.Enums;

namespace Application.Profiles
{
    // Mapeos de Property hacia los distintos DTOs: lista, detalle y creación desde el request.
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            // Mapeo para la lista de propiedades (resumen)
            CreateMap<Property, PropertyListResponseDTO>()
                // Mapea la imagen principal (si existe)
                .ForMember(dest => dest.MainImage,
                           opt => opt.MapFrom(src =>
                               src.Images.FirstOrDefault(i => i.IsMainImage)))

                // Calcula el promedio de reseñas (si hay reseñas)
                .ForMember(dest => dest.AverageRating,
                           opt => opt.MapFrom(src =>
                               src.Reviews.Any()
                                   ? src.Reviews.Average(r => r.Rating)
                                   : 0))

                // Mapea las propiedades de Address aplanadas
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address!.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address!.State))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address!.Country))
                .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(src => src.Address!.StreetAddress))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address!.PostalCode));

            // Mapeo detallado (para ver una propiedad completa)
            CreateMap<Property, PropertyDetailsResponseDTO>()
                .ForMember(dest => dest.AverageRating,
                    opt => opt.MapFrom(src =>
                        src.Reviews.Any()
                            ? src.Reviews.Average(r => r.Rating)
                            : 0))
                .ForMember(dest => dest.Amenities,
                    opt => opt.MapFrom(src => src.Amenities))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Reviews,
                    opt => opt.MapFrom(src => src.Reviews))
                .ForMember(dest => dest.AvailableRanges,
                    opt => opt.MapFrom(src => src.Availabilities))
                .ForMember(dest => dest.ReservedRanges,
                    opt => opt.MapFrom(src => src.Reservations
                        .Where(r => r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.PendingPayment || r.Status == ReservationStatus.PaymentUploaded)))

                // Mapea las propiedades de Address aplanadas
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address!.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address!.State))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address!.Country))
                .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(src => src.Address!.StreetAddress))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address!.PostalCode));

            // Mapeo de PropertyRequestDTO a Property (crear Address a partir de propiedades aplanadas)
            CreateMap<PropertyRequestDTO, Property>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
                    new Domain.ValueObjects.Address(
                        src.Country,
                        src.State,
                        src.City,
                        src.PostalCode,
                        src.StreetAddress
                    )));

        }
    }
}
