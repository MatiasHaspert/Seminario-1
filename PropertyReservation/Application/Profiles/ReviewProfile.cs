using AutoMapper;
using Domain.Entities;
using Application.DTOs.Review;

namespace Application.Profiles
{
    // Mapeos de Review hacia su DTO de respuesta y desde el DTO de solicitud.
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            // Incluye el nombre y el email del autor de la reseña
            CreateMap<Review, ReviewResponseDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Title));

            // Asigna la fecha actual al crear una reseña
            CreateMap<ReviewRequestDTO, Review>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
