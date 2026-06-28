using AutoMapper;
using Infrastructure.Repositories;
using Application.DTOs;
using Application.DTOs.Property;
using Domain.Entities;

namespace Application.Services
{
    // Gestiona las operaciones principales sobre propiedades: creación, búsqueda, edición y baja.
    public class PropertyService
    {
        private readonly PropertyRepository _propertyRepository;
        private readonly AmenityRepository _amenityRepository;
        private readonly CurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public PropertyService(
            PropertyRepository propertyRepository,
            AmenityRepository amenityRepository,
            CurrentUserService currentUserService,
            IMapper mapper
        )
        {
            _propertyRepository = propertyRepository;
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PropertyDetailsResponseDTO> GetPropertyByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            return _mapper.Map<PropertyDetailsResponseDTO>(property);
        }

        public async Task<IEnumerable<PropertyListResponseDTO>> GetPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyListResponseDTO>>(properties);
        }

        public async Task<PropertyListResponseDTO> CreatePropertyAsync(PropertyRequestDTO propertyDTO)
        {
            var ownerId = _currentUserService.UserId;

            Property property = _mapper.Map<Property>(propertyDTO);

            property.OwnerId = ownerId;

            if (propertyDTO.AmenityIds.Any())
            {
                property.Amenities = await _amenityRepository.GetByIdsAsync(propertyDTO.AmenityIds);
            }

            await _propertyRepository.AddAsync(property);
            return _mapper.Map<PropertyListResponseDTO>(property);
        }

        public async Task<IEnumerable<PropertyListResponseDTO>> SearchPropertiesAsync(PropertySearchRequestDTO request)
        {
            var properties = await _propertyRepository.SearchAsync(
                request.City,
                request.Guests,
                request.CheckIn,
                request.CheckOut
            );
            return _mapper.Map<IEnumerable<PropertyListResponseDTO>>(properties);
        }

        public async Task PutPropertyAsync(int id, PropertyRequestDTO propertyDTO)
        {
            var ownerId = _currentUserService.UserId;

            var property = await _propertyRepository.GetByIdWithAmenitiesAsync(id);

            // Validar que la propiedad exista
            if (property == null)
                throw new KeyNotFoundException("Propiedad no encontrada.");

            // Validar que el usuario actual sea el dueño de la propiedad
            if (property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tienes permiso para modificar esta propiedad.");

            _mapper.Map(propertyDTO, property);

            // Reemplazar amenities solo si se enviaron IDs; si no, se conservan los actuales
            if (propertyDTO.AmenityIds.Any())
            {
                property.Amenities = await _amenityRepository.GetByIdsAsync(propertyDTO.AmenityIds);
            }
            await _propertyRepository.UpdateAsync(property);
        }

        public async Task<IEnumerable<PropertyListResponseDTO>> GetPropertiesByCurrentOwnerAsync()
        {
            var ownerId = _currentUserService.UserId;

            var properties = await _propertyRepository.GetByOwnerIdAsync(ownerId);

            return _mapper.Map<IEnumerable<PropertyListResponseDTO>>(properties);
        }

        public async Task DeleteSafePropertyAsync(int propertyId)
        {
            var ownerId = _currentUserService.UserId;

            var property = await _propertyRepository.GetByIdWithReservationsAsync(propertyId);
            if (property == null)
                throw new KeyNotFoundException("Propiedad no encontrada.");

            if (!_currentUserService.IsAdmin && property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta propiedad.");

            // Validar que no tenga reservas activas a futuro (las canceladas o completadas no bloquean la baja)
            var now = DateOnly.FromDateTime(DateTime.Now);
            if (property.Reservations.Any(r => r.IsActive && r.EndDate >= now))
                throw new InvalidOperationException("No se puede eliminar la propiedad porque tiene reservas activas a futuro.");

            property.IsDeleted = true;

            await _propertyRepository.UpdateAsync(property);
        }

        public async Task<IEnumerable<PropertyListResponseDTO>> GetAllPropertiesIncludingDeletedAsync()
        {
            var properties = await _propertyRepository.GetAllIncludingDeletedAsync();
            return _mapper.Map<IEnumerable<PropertyListResponseDTO>>(properties);
        }

        public async Task RestorePropertyAsync(int propertyId)
        {
            var property = await _propertyRepository.GetByIdIgnoringDeletedFilterAsync(propertyId);
            if (property == null)
                throw new KeyNotFoundException("Propiedad no encontrada.");
            if (!property.IsDeleted)
                throw new InvalidOperationException("La propiedad no está eliminada.");

            property.IsDeleted = false;
            await _propertyRepository.UpdateAsync(property);
        }
    }
}
