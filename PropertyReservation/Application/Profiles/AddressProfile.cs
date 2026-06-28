using AutoMapper;
using Domain.ValueObjects;
using Application.DTOs;

namespace Application.Profiles
{
    // Mapeo bidireccional entre Address (value object del dominio) y AddressDTO.
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
