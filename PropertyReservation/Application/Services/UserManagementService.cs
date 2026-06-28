using Application.DTOs.User;
using AutoMapper;
using Infrastructure.Repositories;
using Domain.Entities;

namespace Application.Services
{
    // Permite al administrador listar, consultar y gestionar los usuarios del sistema.
    public class UserManagementService
    {
        private readonly UserRepository _userRepository;
        private readonly CurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public UserManagementService(
            UserRepository userRepository,
            CurrentUserService currentUser,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserListDTO>> GetAllAsync(string? emailFilter, string? roleFilter, bool? isActiveFilter)
        {
            IEnumerable<User> users = await _userRepository.GetByEmailOrRolOrActive(emailFilter, roleFilter, isActiveFilter);

            return _mapper.Map<IEnumerable<UserListDTO>>(users);
        }

        public async Task<UserDetailDTO> GetByIdAsync(int userId)
        {
            User? user = await _userRepository.GetByIdWithDetails(userId);

            if (user is null)
                throw new KeyNotFoundException($"No se encontró el usuario con Id {userId}.");

            return _mapper.Map<UserDetailDTO>(user);
        }

        public async Task UpdateRoleAsync(int userId, UpdateUserRoleDTO dto)
        {
            User? user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                throw new KeyNotFoundException($"No se encontró el usuario con Id {userId}.");

            if (userId == _currentUser.UserId)
                throw new InvalidOperationException("No puede cambiar su propio rol.");

            if (user.Role == dto.Role)
                throw new ArgumentException($"El usuario ya tiene el rol {dto.Role}.");

            user.Role = dto.Role;
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateStatusAsync(int userId, UpdateUserStatusDTO dto)
        {
            User? user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                throw new KeyNotFoundException($"No se encontró el usuario con Id {userId}.");

            // El Administrador no puede deshabilitarse a sí mismo.
            if (userId == _currentUser.UserId && !dto.IsActive)
                throw new InvalidOperationException("No puede deshabilitarse a sí mismo.");

            if (user.IsActive == dto.IsActive)
                return;

            user.IsActive = dto.IsActive;
            await _userRepository.UpdateAsync(user);
        }
    }
}
