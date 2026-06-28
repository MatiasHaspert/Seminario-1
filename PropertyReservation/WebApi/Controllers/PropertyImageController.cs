using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs.PropertyImage;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    /// <summary>
    /// Imágenes de las propiedades: consulta pública y operaciones de carga, marcado de
    /// imagen principal y baja, reservadas al dueño de la propiedad.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PropertyImageController : ControllerBase
    {
        private readonly PropertyImageService _imageService;

        public PropertyImageController(PropertyImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>Lista las imágenes de una propiedad.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <param name="propertyId">Identificador de la propiedad.</param>
        /// <response code="200">Imágenes de la propiedad.</response>
        /// <response code="404">No existe la propiedad.</response>
        [AllowAnonymous]
        [HttpGet("{propertyId}")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImageResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImagesByProperty(int propertyId)
        {
            try
            {
                var images = await _imageService.GetImagesByPropertyAsync(propertyId);
                return Ok(images);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Sube una o más imágenes a una propiedad.</summary>
        /// <remarks>
        /// Solo el dueño de la propiedad puede subir imágenes. Se envía como
        /// <c>multipart/form-data</c>. Formatos permitidos: jpg, jpeg, png y webp.
        /// Si la propiedad aún no tiene imagen principal, la primera subida se marca como tal.
        /// </remarks>
        /// <param name="propertyId">Identificador de la propiedad.</param>
        /// <param name="files">Archivos de imagen a subir.</param>
        /// <response code="200">Imágenes subidas correctamente.</response>
        /// <response code="400">La propiedad no existe.</response>
        /// <response code="409">No se enviaron archivos o algún formato no está permitido.</response>
        [HttpPost("{propertyId}")]
        [Authorize(Roles = "Owner")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(List<PropertyImageResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UploadImages(int propertyId, List<IFormFile> files)
        {
            try
            {
                var images = await _imageService.UploadPropertyImagesAsync(propertyId, files, Request);
                return Ok(images);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        /// <summary>Marca una imagen como la principal de la propiedad.</summary>
        /// <remarks>Solo el dueño de la propiedad puede hacerlo.</remarks>
        /// <param name="id">Identificador de la imagen.</param>
        /// <response code="200">Imagen marcada como principal.</response>
        /// <response code="404">No existe la imagen.</response>
        [HttpPut("main/{id}")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(typeof(PropertyImageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetMainImage(int id)
        {
            try
            {
                var result = await _imageService.SetMainImageAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
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

        /// <summary>Elimina una imagen de una propiedad.</summary>
        /// <remarks>
        /// Solo el dueño de la propiedad puede eliminarla. Si era la imagen principal,
        /// se promueve automáticamente la siguiente imagen más antigua.
        /// </remarks>
        /// <param name="id">Identificador de la imagen a eliminar.</param>
        /// <response code="204">Imagen eliminada correctamente.</response>
        /// <response code="404">No existe la imagen.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                await _imageService.DeleteImageAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

    }
}
