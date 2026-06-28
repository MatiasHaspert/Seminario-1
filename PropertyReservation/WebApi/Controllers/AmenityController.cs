using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Amenity;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// Servicios (*amenities*) que pueden asociarse a las propiedades. La consulta es
    /// pública; el alta, la edición y la baja están reservadas al administrador.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AmenityController : ControllerBase
    {
        private readonly AmenityService _amenityService;

        public AmenityController(AmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        /// <summary>Lista los servicios disponibles.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <response code="200">Listado de servicios.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AmenityResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AmenityResponseDTO>>> GetAmenities()
        {
            return Ok(await _amenityService.GetAllAmenitiesAsync());
        }

        /// <summary>Crea un nuevo servicio.</summary>
        /// <param name="dto">Datos del servicio a crear.</param>
        /// <response code="201">Servicio creado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">Ya existe un servicio con ese nombre.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(AmenityResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AmenityResponseDTO>> CreateAmenity([FromBody] AmenityRequestDTO dto)
        {
            try
            {
                var created = await _amenityService.CreateAmenityAsync(dto);
                return CreatedAtAction(nameof(GetAmenities), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>Actualiza el nombre de un servicio.</summary>
        /// <param name="id">Identificador del servicio.</param>
        /// <param name="dto">Nuevo nombre del servicio.</param>
        /// <response code="204">Servicio actualizado correctamente.</response>
        /// <response code="404">No existe el servicio.</response>
        /// <response code="409">Ya existe otro servicio con ese nombre, o el servicio está asignado a una o más propiedades.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAmenity(int id, [FromBody] AmenityRequestDTO dto)
        {
            try
            {
                await _amenityService.UpdateAmenityAsync(id, dto);
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
        }

        /// <summary>Elimina un servicio.</summary>
        /// <param name="id">Identificador del servicio a eliminar.</param>
        /// <response code="204">Servicio eliminado correctamente.</response>
        /// <response code="404">No existe el servicio.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            try
            {
                await _amenityService.DeleteAmenityAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
