using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// Reseñas de las propiedades: consulta pública, alta y edición por parte del autor
    /// (huésped) y moderación (baja) reservada al administrador.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService ReviewService)
        {
            _reviewService = ReviewService;
        }

        /// <summary>Lista todas las reseñas del sistema (moderación).</summary>
        /// <remarks>Solo accesible para el administrador.</remarks>
        /// <response code="200">Todas las reseñas ordenadas por fecha descendente.</response>
        [HttpGet("all")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetAllReviews()
        {
            return Ok(await _reviewService.GetAllReviewsAsync());
        }

        /// <summary>Lista las reseñas de una propiedad.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <param name="propertyId">Identificador de la propiedad.</param>
        /// <response code="200">Reseñas de la propiedad.</response>
        /// <response code="404">No existe la propiedad.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetPropertyReviews([FromQuery] int propertyId)
        {
            try
            {
                var reviews = await _reviewService.GetPropertyReviewsAsync(propertyId);
                return Ok(reviews);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Obtiene una reseña por su identificador.</summary>
        /// <remarks>Endpoint público.</remarks>
        /// <param name="id">Identificador de la reseña.</param>
        /// <response code="200">Detalle de la reseña.</response>
        /// <response code="404">No existe la reseña.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReviewResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDTO>> GetReview(int id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                return Ok(review);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>Edita una reseña propia.</summary>
        /// <remarks>Solo el autor de la reseña puede modificarla.</remarks>
        /// <param name="id">Identificador de la reseña.</param>
        /// <param name="reviewRequestDTO">Nuevos datos de la reseña.</param>
        /// <response code="204">Reseña actualizada correctamente.</response>
        /// <response code="404">No existe la reseña.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReview(int id, ReviewRequestDTO reviewRequestDTO)
        {
            try
            {
                await _reviewService.UpdateReviewAsync(id, reviewRequestDTO);
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
        }

        /// <summary>Elimina una reseña inapropiada (moderación).</summary>
        /// <param name="id">Identificador de la reseña a eliminar.</param>
        /// <response code="204">Reseña eliminada correctamente.</response>
        /// <response code="404">No existe la reseña.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Crea una reseña para una propiedad.</summary>
        /// <remarks>
        /// Solo los huéspedes que completaron una estadía en la propiedad pueden dejar una reseña.
        /// </remarks>
        /// <param name="reviewRequestDTO">Calificación, comentario y propiedad reseñada.</param>
        /// <response code="201">Reseña creada correctamente.</response>
        /// <response code="400">El usuario no completó una estadía o ya reseñó esta propiedad.</response>
        /// <response code="404">No existe la propiedad.</response>
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(ReviewResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDTO>> PostReview(ReviewRequestDTO reviewRequestDTO)
        {
            try
            {
                var createdReview = await _reviewService.CreateReviewAsync(reviewRequestDTO);
                return CreatedAtAction("GetReview", new { id = createdReview.Id, propertyId = createdReview.PropertyId }, createdReview);
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
