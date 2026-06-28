using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    // Mapeos de User hacia su DTO de respuesta y desde el DTO de registro.
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Convierte el enum Role a su representación en texto
            CreateMap<User, UserResponseDTO>()
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString())
                );

            // La contraseña se hashea en el servicio, no en el mapeo
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserListDTO>()
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString())
                );

            CreateMap<User, UserDetailDTO>()
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString())
                )
                .ForMember(
                    dest => dest.ReservationsCount,
                    opt => opt.MapFrom(src => src.Reservations.Count)
                )
                .ForMember(
                    dest => dest.PropertiesCount,
                    opt => opt.MapFrom(src => src.Properties.Count)
                );
        }
    }
}
