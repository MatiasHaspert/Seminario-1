using AutoMapper;
using Application.DTOs.Reservation;
using Domain.Entities;

namespace Application.Profiles
{
    // Mapeos de Reservation hacia los distintos DTOs de respuesta y vista pública.
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            // Mapeo completo con datos de la propiedad y el huésped
            CreateMap<Reservation, ReservationResponseDTO>()
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title))
                .ForMember(dest => dest.PropertyImageUrl, opt => opt.MapFrom(src =>
                    src.Property.Images.Any(img => img.IsMainImage)
                        ? src.Property.Images.First(img => img.IsMainImage).Url
                        : string.Empty))
                .ForMember(dest => dest.GuestName, opt => opt.MapFrom(src => (src.Guest.Name + " " + src.Guest.LastName).Trim()));

            // Mapeo simple para crear la reserva desde el request
            CreateMap<ReservationRequestDTO, Reservation>();

            // Sólo expone las fechas al público
            CreateMap<Reservation, ReservationPublicDTO>();

            // Vista resumida para el listado de reservas del usuario
            CreateMap<Reservation, MyReservationResponseDTO>()
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title))
                .ForMember(dest => dest.PropertyImageUrl, opt => opt.MapFrom(src =>
                    src.Property.Images.Any(img => img.IsMainImage)
                        ? src.Property.Images.First(img => img.IsMainImage).Url
                        : string.Empty));
        }
    }
}
