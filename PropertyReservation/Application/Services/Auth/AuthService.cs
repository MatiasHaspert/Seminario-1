using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using System.Security.Claims;

namespace Application.Services.Auth
{
    // Maneja el registro, login y obtención del usuario autenticado.
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly JwtTokenGeneratorService _jwtTokenGeneratorService;
        private readonly IMapper _mapper;
        private readonly CurrentUserService currentUserService;

        public AuthService(
            UserRepository userRepository,
            PasswordService passwordService,
            JwtTokenGeneratorService jwtTokenGeneratorService,
            IMapper mapper,
            CurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtTokenGeneratorService = jwtTokenGeneratorService;
            _mapper = mapper;
            this.currentUserService = currentUserService;
        }

        // Registra un nuevo usuario en el sistema.
        public async Task<UserResponseDTO> RegisterAsync(UserRegisterDTO registerDto)
        {
            // Verificar si el email ya existe
            if (await _userRepository.ExistsWithEmailAsync(registerDto.Email))
            {
                throw new InvalidOperationException($"El email '{registerDto.Email}' ya está en uso.");
            }

            // Hashear la contraseña
            var hashedPassword = _passwordService.HashPassword(registerDto.Password);

            // Solo se permite auto-registrarse como Huésped (User) o Propietario (Owner).
            // Admin jamás puede asignarse desde el registro público (endpoint anónimo).
            if (registerDto.role != Role.User && registerDto.role != Role.Owner)
            {
                throw new InvalidOperationException("El tipo de cuenta seleccionado no es válido.");
            }

            var user = new User
            {
                Name = registerDto.Name,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                Phone = registerDto.Phone ?? string.Empty,
                Role = registerDto.role
            };

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserResponseDTO>(user);
        }


        // Autentica un usuario y devuelve un token JWT.
        public async Task<LoginResponseDTO> LoginAsync(UserLoginDTO loginDto)
        {
            // Buscar usuario por email
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");
            }

            // Verificar la contraseña
            if (!_passwordService.VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");
            }

            // Generar el token JWT
            var token = _jwtTokenGeneratorService.GenerateToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                User = _mapper.Map<UserResponseDTO>(user)
            };
        }

        public async Task<UserResponseDTO> GetCurrentUserAsync()
        {
            var userId = currentUserService.UserId;
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("Usuario no autenticado.");

            return _mapper.Map<UserResponseDTO>(user);
        }
    }
}
