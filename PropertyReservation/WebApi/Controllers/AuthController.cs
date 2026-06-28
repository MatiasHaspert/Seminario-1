using Application.DTOs.User;
using Application.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Autenticación y registro de usuarios. Expone el alta de cuentas, el inicio de sesión
    /// (que devuelve el token JWT) y la consulta del usuario autenticado.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Registra un nuevo usuario en el sistema.</summary>
        /// <remarks>
        /// Endpoint público. El rol (`User` u `Owner`) se indica en el cuerpo; el email debe ser único.
        /// </remarks>
        /// <param name="request">Datos de alta del usuario.</param>
        /// <response code="200">Usuario registrado correctamente.</response>
        /// <response code="400">Datos inválidos o el email ya está en uso.</response>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] UserRegisterDTO request)
        {
            try
            {
                var userResponseDTO = await _authService.RegisterAsync(request);
                return Ok(userResponseDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Inicia sesión y devuelve el token JWT.</summary>
        /// <remarks>
        /// Endpoint público. Copiá el campo `token` de la respuesta y usalo en **Authorize** 🔒
        /// para acceder a los endpoints protegidos.
        /// </remarks>
        /// <param name="request">Credenciales de acceso (email y contraseña).</param>
        /// <response code="200">Autenticación exitosa: devuelve el token y los datos del usuario.</response>
        /// <response code="401">Credenciales inválidas o cuenta deshabilitada.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] UserLoginDTO request)
        {
            try
            {
                var loginResponseDTO = await _authService.LoginAsync(request);
                return Ok(loginResponseDTO);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>Devuelve los datos del usuario autenticado actualmente.</summary>
        /// <remarks>Requiere un token JWT válido en el header <c>Authorization</c>.</remarks>
        /// <response code="200">Datos del usuario autenticado.</response>
        /// <response code="401">No autenticado o token inválido.</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponseDTO>> GetCurrentUser()
        {
            try
            {
                var userResponseDTO = await _authService.GetCurrentUserAsync();
                return Ok(userResponseDTO);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


    }
}
