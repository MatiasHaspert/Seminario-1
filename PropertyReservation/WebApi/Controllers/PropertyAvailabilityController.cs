using Microsoft.AspNetCore.Mvc;
using Application.DTOs.PropertyAvailability;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// Rangos de disponibilidad de cada propiedad. La consulta es pública; la creación,
    /// edición y baja de rangos quedan reservadas al dueño de la propiedad.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PropertyAvailabilityController : ControllerBase
    {
        private readonly PropertyAvailabilityService _propertyAvailabilityService;

        public PropertyAvailabilityController(PropertyAvailabilityService propertyAvailabilityService)
        {
            _propertyAvailabilityService = propertyAvailabilityService;
        }

        /// <summary>Lista los rangos de disponibilidad de una propiedad.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <param name="propertyId">Identificador de la propiedad.</param>
        /// <response code="200">Rangos de disponibilidad de la propiedad.</response>
        /// <response code="404">No existe la propiedad.</response>
        [AllowAnonymous]
        [HttpGet("{propertyId}")]
        [ProducesResponseType(typeof(IEnumerable<PropertyAvailabilityResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PropertyAvailabilityResponseDTO>>> GetPropertyAvailabilities(int propertyId)
        {
            try
            {
                return Ok(await _propertyAvailabilityService.GetPropertyAvailabilitiesAsync(propertyId));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Actualiza un rango de disponibilidad existente.</summary>
        /// <remarks>Solo el dueño de la propiedad puede modificarlo.</remarks>
        /// <param name="availabilityId">Identificador del rango de disponibilidad.</param>
        /// <param name="propertyAvailabilityDTO">Nuevas fechas del rango.</param>
        /// <response code="204">Rango actualizado correctamente.</response>
        /// <response code="400">Datos inválidos (por ejemplo, fechas incoherentes).</response>
        /// <response code="409">El rango se solapa con otro existente.</response>
        [HttpPut("{availabilityId}")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutPropertyAvailability(int availabilityId, PropertyAvailabilityRequestDTO propertyAvailabilityDTO)
        {
            try
            {
                await _propertyAvailabilityService.UpdatePropertyAvailabilityAsync(availabilityId, propertyAvailabilityDTO);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>Crea un nuevo rango de disponibilidad para una propiedad.</summary>
        /// <remarks>Solo el dueño de la propiedad puede crearlo.</remarks>
        /// <param name="propertyAvailabilityDTO">Propiedad y fechas del nuevo rango.</param>
        /// <response code="201">Rango creado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">El rango se solapa con otro existente.</response>
        [HttpPost]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(typeof(PropertyAvailabilityResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PropertyAvailabilityResponseDTO>> PostPropertyAvailability(PropertyAvailabilityRequestDTO propertyAvailabilityDTO)
        {
            try
            {
                var createdAvailability = await _propertyAvailabilityService.CreatePropertyAvailabilityAsync(propertyAvailabilityDTO);
                return CreatedAtAction("GetPropertyAvailabilities", new { propertyId = createdAvailability.PropertyId }, createdAvailability);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }

        }

        /// <summary>Elimina un rango de disponibilidad.</summary>
        /// <remarks>Solo el dueño puede eliminarlo, y siempre que no tenga reservas activas asociadas.</remarks>
        /// <param name="id">Identificador del rango de disponibilidad.</param>
        /// <response code="204">Rango eliminado correctamente.</response>
        /// <response code="404">No existe el rango de disponibilidad.</response>
        /// <response code="409">El rango tiene reservas activas y no puede eliminarse.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeletePropertyAvailability(int id)
        {
            try
            {
                await _propertyAvailabilityService.DeletePropertyAvailabilityAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
