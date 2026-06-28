using AutoMapper;
using Infrastructure.Repositories;
using Application.DTOs.Amenity;
using Domain.Entities;

namespace Application.Services
{
    // Gestiona las operaciones de CRUD sobre los servicios/amenities disponibles.
    public class AmenityService
    {
        private readonly AmenityRepository _amenityRepository;
        private readonly IMapper _mapper;

        public AmenityService(AmenityRepository amenityRepository, IMapper mapper)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<AmenityResponseDTO>> GetAllAmenitiesAsync()
        {
            var amenities = await _amenityRepository.GetAllAsync();
            return amenities.Select(a => _mapper.Map<AmenityResponseDTO>(a)).ToList();
        }

        public async Task<AmenityResponseDTO> CreateAmenityAsync(AmenityRequestDTO amenityRequestDTO)
        {
            var nameExists = await _amenityRepository.ExistsByNameAsync(amenityRequestDTO.Name);
            if (nameExists)
                throw new InvalidOperationException("Ya existe un amenity con ese nombre.");

            var amenity = _mapper.Map<Amenity>(amenityRequestDTO);
            await _amenityRepository.AddAsync(amenity);
            return _mapper.Map<AmenityResponseDTO>(amenity);
        }

        public async Task UpdateAmenityAsync(int amenityId, AmenityRequestDTO amenityRequestDTO)
        {
            var amenityExists = await _amenityRepository.ExistsAsync(amenityId);
            if (!amenityExists)
                throw new ArgumentException("Servicio no encontrado.");

            var isAssigned = await _amenityRepository.IsAssignedToAnyPropertyAsync(amenityId);
            if (isAssigned)
                throw new InvalidOperationException("El amenity está asignado a una o más propiedades y no puede modificarse.");

            var nameExists = await _amenityRepository.ExistsByNameAsync(amenityRequestDTO.Name, excludeId: amenityId);
            if (nameExists)
                throw new InvalidOperationException("Ya existe un amenity con ese nombre.");

            var amenity = _mapper.Map<Amenity>(amenityRequestDTO);
            amenity.Id = amenityId;
            await _amenityRepository.UpdateAsync(amenity);
        }

        public async Task DeleteAmenityAsync(int amenityId)
        {
            // Cargar la entidad antes de eliminarla para que EF la rastree
            var amenity = await _amenityRepository.GetByIdAsync(amenityId);
            if (amenity == null)
            {
                throw new ArgumentException("Servicio no encontrado.");
            }
            await _amenityRepository.DeleteAsync(amenity);
        }

    }
}
