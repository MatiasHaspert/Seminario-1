using Application.DTOs.User;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Gestión de usuarios del sistema (panel de administración): listado con filtros,
    /// detalle, cambio de rol y habilitación/baja lógica de cuentas. Reservado a administradores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController(UserManagementService userManagementService) : ControllerBase
    {
        private readonly UserManagementService _userManagementService = userManagementService;

        /// <summary>Lista los usuarios con filtros opcionales.</summary>
        /// <param name="email">Filtrar por email (coincidencia).</param>
        /// <param name="role">Filtrar por rol (<c>User</c>, <c>Owner</c> o <c>Admin</c>).</param>
        /// <param name="isActive">Filtrar por estado de habilitación.</param>
        /// <response code="200">Usuarios que cumplen los filtros.</response>
        /// <response code="400">Algún filtro tiene un valor inválido.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(IEnumerable<UserListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserListDTO>>> GetUsers(
            [FromQuery] string? email,
            [FromQuery] string? role,
            [FromQuery] bool? isActive)
        {
            try
            {
                var users = await _userManagementService.GetAllAsync(email, role, isActive);
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Obtiene el detalle de un usuario por su identificador.</summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <response code="200">Detalle del usuario.</response>
        /// <response code="404">No existe el usuario.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _userManagementService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Cambia el rol de un usuario.</summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <param name="dto">Nuevo rol a asignar.</param>
        /// <response code="204">Rol actualizado correctamente.</response>
        /// <response code="400">El rol indicado es inválido.</response>
        /// <response code="404">No existe el usuario.</response>
        /// <response code="409">El cambio de rol no está permitido para este usuario.</response>
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleDTO dto)
        {
            try
            {
                await _userManagementService.UpdateRoleAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Habilita o deshabilita (baja lógica) la cuenta de un usuario.</summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <param name="dto">Estado de habilitación a aplicar.</param>
        /// <response code="204">Estado actualizado correctamente.</response>
        /// <response code="404">No existe el usuario.</response>
        /// <response code="409">La operación no está permitida para este usuario.</response>
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UpdateUserStatusDTO dto)
        {
            try
            {
                await _userManagementService.UpdateStatusAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
