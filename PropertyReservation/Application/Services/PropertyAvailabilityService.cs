using AutoMapper;
using Infrastructure.Repositories;
using Application.DTOs.PropertyAvailability;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    // Administra los rangos de disponibilidad que el dueño define para cada propiedad.
    public class PropertyAvailabilityService
    {
        private readonly PropertyAvailabilityRepository _availabilityRepository;
        private readonly PropertyRepository _propertyRepository;
        private readonly CurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public PropertyAvailabilityService(
            PropertyAvailabilityRepository availabilityRepository,
            PropertyRepository propertyRepository,
            IMapper mapper,
            CurrentUserService currentUserService)
        {
            _availabilityRepository = availabilityRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<PropertyAvailabilityResponseDTO>> GetPropertyAvailabilitiesAsync(int propertyId)
        {
            if (!await _propertyRepository.ExistsAsync(propertyId))
                throw new ArgumentException("La propiedad indicada no existe.");

            var availabilities = await _availabilityRepository.GetByPropertyIdAsync(propertyId);
            return availabilities.Select(a => _mapper.Map<PropertyAvailabilityResponseDTO>(a)).ToList();
        }

        public async Task<PropertyAvailabilityResponseDTO> CreatePropertyAvailabilityAsync(PropertyAvailabilityRequestDTO availabilityDto)
        {
            var ownerId = _currentUserService.UserId;

            // Validar fechas
            ValidateAvailabilityDates(availabilityDto.StartDate, availabilityDto.EndDate);

            // Verificar existencia de la propiedad
            var property = await _propertyRepository.GetByIdAsync(availabilityDto.PropertyId);
            if (property == null)
                throw new InvalidOperationException("No se puede crear la disponibilidad: la propiedad no existe.");

            // Verificar que el usuario actual es el propietario de la propiedad
            if (property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tiene permiso para crear disponibilidades para esta propiedad.");

            // Verificar solapamiento de fechas
            var hasOverlap = await _availabilityRepository.HasOverlapAsync(
                availabilityDto.PropertyId,
                availabilityDto.StartDate,
                availabilityDto.EndDate);

            if (hasOverlap)
                throw new InvalidOperationException("La disponibilidad se solapa con una existente.");

            var availability = _mapper.Map<PropertyAvailability>(availabilityDto);
            await _availabilityRepository.AddAsync(availability);
            return _mapper.Map<PropertyAvailabilityResponseDTO>(availability);
        }

        public async Task UpdatePropertyAvailabilityAsync(int availabilityId, PropertyAvailabilityRequestDTO availabilityDto)
        {
            var ownerId = _currentUserService.UserId;

            // Validar fechas
            ValidateAvailabilityDates(availabilityDto.StartDate, availabilityDto.EndDate);

            // Verificar existencia de la disponibilidad
            var availabilityExists = await _availabilityRepository.ExistsAsync(availabilityId);
            if (!availabilityExists)
                throw new ArgumentException("La disponibilidad indicada no existe.");

            // Verificar existencia de la propiedad 
            var property = await _propertyRepository.GetByIdAsync(availabilityDto.PropertyId);
            if (property == null)
                throw new InvalidOperationException("No se puede actualizar la disponibilidad: la propiedad no existe.");

            // Verificar que el usuario actual es el propietario de la propiedad
            if (property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tiene permiso para actualizar la disponibilidad de esta propiedad.");

            // Verificar solapamiento de fechas, excluyendo la disponibilidad que se está actualizando
            var hasOverlap = await _availabilityRepository.HasOverlapAsync(
                availabilityDto.PropertyId,
                availabilityDto.StartDate,
                availabilityDto.EndDate,
                availabilityId);

            if (hasOverlap)
                throw new InvalidOperationException("La disponibilidad se solapa con una existente.");

            var availability = _mapper.Map<PropertyAvailability>(availabilityDto);
            availability.Id = availabilityId; // Asegurar que el ID se mantiene para la actualización
            await _availabilityRepository.UpdateAsync(availability);
        }

        public async Task DeletePropertyAvailabilityAsync(int availabilityId)
        {
            var ownerId = _currentUserService.UserId;

            // Obtener la disponibilidad con la propiedad y sus reservas
            var availability = await _availabilityRepository.GetByIdWithPropertyAndReservationsAsync(availabilityId);

            // Verificar existencia de la disponibilidad
            if (availability == null)
                throw new ArgumentException("La disponibilidad indicada no existe.");

            // Verificar que el usuario actual es el propietario de la propiedad
            if (availability.Property.OwnerId != ownerId)
                throw new UnauthorizedAccessException("No tiene permiso para eliminar la disponibilidad de esta propiedad.");

            // Verificar si hay reservas conflictivas usando el helper de Property
            if (availability.Property.HasConflictingReservation(availability.StartDate, availability.EndDate))
                throw new InvalidOperationException("No se puede eliminar la disponibilidad porque existen reservas confirmadas o pendientes dentro del rango.");

            // Eliminar si no hay conflictos
            await _availabilityRepository.DeleteAsync(availability);
        }

        private void ValidateAvailabilityDates(DateOnly start, DateOnly end)
        {
            if (end <= start)
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

            double duration = end.DayNumber - start.DayNumber;
            if (duration > 365)
                throw new ArgumentException("La disponibilidad no puede superar un año.");
            if (duration < 1)
                throw new ArgumentException("La disponibilidad debe durar al menos un día.");
        }

    }
}
