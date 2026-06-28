using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Property;
using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// Gestión de propiedades: listado y búsqueda públicos, alta/edición/baja para dueños
    /// y administradores, y restauración de bajas lógicas reservada al administrador.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PropertyController : ControllerBase
    {
        private readonly PropertyService _PropertyService;

        public PropertyController(PropertyService PropertyService)
        {
            _PropertyService = PropertyService;
        }

        /// <summary>Lista las propiedades del dueño autenticado.</summary>
        /// <response code="200">Listado de propiedades del dueño.</response>
        [HttpGet("my")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(typeof(IEnumerable<PropertyListResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PropertyListResponseDTO>>> GetMyProperties()
        {
            var properties = await _PropertyService.GetPropertiesByCurrentOwnerAsync();
            return Ok(properties);

        }

        /// <summary>Lista las propiedades publicadas.</summary>
        /// <remarks>
        /// Endpoint público. Si <paramref name="includeDeleted"/> es <c>true</c> y el usuario
        /// autenticado es <c>Admin</c>, también incluye las propiedades dadas de baja (soft-delete).
        /// </remarks>
        /// <param name="includeDeleted">Incluir las propiedades eliminadas lógicamente (solo Admin).</param>
        /// <response code="200">Listado de propiedades.</response>
        /// <response code="403">Se pidió <c>includeDeleted=true</c> sin ser administrador.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropertyListResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PropertyListResponseDTO>>> GetProperties(
            [FromQuery] bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                if (!User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var all = await _PropertyService.GetAllPropertiesIncludingDeletedAsync();
                return Ok(all);
            }

            var properties = await _PropertyService.GetPropertiesAsync();
            return Ok(properties);
        }

        /// <summary>Obtiene el detalle completo de una propiedad.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <param name="id">Identificador de la propiedad.</param>
        /// <response code="200">Detalle de la propiedad.</response>
        /// <response code="404">No existe una propiedad con ese identificador.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyDetailsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropertyDetailsResponseDTO>> GetPropertyDetailsById(int id)
        {
            var property = await _PropertyService.GetPropertyByIdAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return property;
        }

        /// <summary>Actualiza una propiedad existente.</summary>
        /// <remarks>El dueño solo puede modificar las suyas; el administrador, cualquiera.</remarks>
        /// <param name="id">Identificador de la propiedad a actualizar.</param>
        /// <param name="property">Nuevos datos de la propiedad.</param>
        /// <response code="204">Propiedad actualizada correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="404">No existe la propiedad.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProperty(int id, [FromBody] PropertyRequestDTO property)
        {
            try
            {
                await _PropertyService.PutPropertyAsync(id, property);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>Crea una nueva propiedad.</summary>
        /// <param name="propertyDTO">Datos de la propiedad a crear.</param>
        /// <response code="201">Propiedad creada. La cabecera <c>Location</c> apunta al detalle.</response>
        /// <response code="400">Datos inválidos.</response>
        [HttpPost]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(typeof(PropertyListResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PropertyListResponseDTO>> PostProperty([FromBody] PropertyRequestDTO propertyDTO)
        {
            try
            {
                PropertyListResponseDTO property = await _PropertyService.CreatePropertyAsync(propertyDTO);
                return CreatedAtAction(nameof(GetPropertyDetailsById), new { id = property.Id }, property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Busca propiedades con filtros opcionales.</summary>
        /// <remarks>Endpoint público. Permite filtrar por ciudad, cantidad de huéspedes y rango de fechas.</remarks>
        /// <param name="searchParams">Filtros de búsqueda (ciudad, huéspedes, check-in y check-out).</param>
        /// <response code="200">Propiedades que cumplen los filtros indicados.</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<PropertyListResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PropertyListResponseDTO>>> SearchProperties([FromQuery] PropertySearchRequestDTO searchParams)
        {
            var properties = await _PropertyService.SearchPropertiesAsync(searchParams);
            return Ok(properties);
        }

        /// <summary>Elimina una propiedad (baja lógica / soft-delete).</summary>
        /// <remarks>El dueño solo puede dar de baja las suyas; el administrador, cualquiera.</remarks>
        /// <param name="propertyId">Identificador de la propiedad a dar de baja.</param>
        /// <response code="204">Propiedad dada de baja correctamente.</response>
        /// <response code="400">La propiedad no puede eliminarse (por ejemplo, tiene reservas activas).</response>
        /// <response code="404">No existe la propiedad.</response>
        [HttpDelete("{propertyId}")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSafeProperty(int propertyId)
        {
            try
            {
                await _PropertyService.DeleteSafePropertyAsync(propertyId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Restaura una propiedad dada de baja lógicamente.</summary>
        /// <param name="propertyId">Identificador de la propiedad a restaurar.</param>
        /// <response code="204">Propiedad restaurada correctamente.</response>
        /// <response code="400">La propiedad no está dada de baja.</response>
        /// <response code="404">No existe la propiedad.</response>
        [HttpPost("{propertyId}/restore")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreProperty(int propertyId)
        {
            try
            {
                await _PropertyService.RestorePropertyAsync(propertyId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
