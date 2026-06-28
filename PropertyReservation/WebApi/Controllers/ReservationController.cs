using Application.DTOs.Reservation;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Ciclo de vida de las reservas: creación por parte del huésped, consultas para
    /// huéspedes, dueños y administradores, y cambios de estado de la reserva.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        /// <summary>Lista las reservas del huésped autenticado.</summary>
        /// <response code="200">Reservas del huésped.</response>
        /// <response code="400">No se pudo determinar el huésped autenticado.</response>
        [Authorize(Roles = "User")]
        [HttpGet("my-reservations")]
        [ProducesResponseType(typeof(IEnumerable<MyReservationResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MyReservationResponseDTO>>> GetMyReservations()
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByUserIdAsync();
                return Ok(reservations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Lista las reservas de una propiedad concreta.</summary>
        /// <remarks>Solo el dueño de la propiedad puede consultarlas.</remarks>
        /// <param name="propertyId">Identificador de la propiedad.</param>
        /// <response code="200">Reservas de la propiedad.</response>
        /// <response code="400">La propiedad no pertenece al dueño autenticado o no existe.</response>
        [Authorize(Roles = "Owner")]
        [HttpGet("by-property/{propertyId}")]
        [ProducesResponseType(typeof(IEnumerable<ReservationResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetReservationsByPropertyId(int propertyId)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByPropertyIdForOwnerIdAsync(propertyId);
                return Ok(reservations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>Lista todas las reservas de las propiedades del dueño autenticado.</summary>
        /// <response code="200">Reservas de todas las propiedades del dueño.</response>
        /// <response code="400">No se pudo determinar el dueño autenticado.</response>
        [Authorize(Roles = "Owner")]
        [HttpGet("owner")]
        [ProducesResponseType(typeof(IEnumerable<ReservationResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetReservationsForOwner()
        {
            try
            {
                var reservations = await _reservationService.GetReservationsForOwnerIdAsync();
                return Ok(reservations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Crea una nueva reserva.</summary>
        /// <remarks>Solo los huéspedes pueden reservar. La reserva nace en estado <c>PendingPayment</c>.</remarks>
        /// <param name="reservationDTO">Propiedad, fechas y cantidad de huéspedes.</param>
        /// <response code="200">Reserva creada correctamente.</response>
        /// <response code="400">Datos inválidos (fechas, capacidad, etc.).</response>
        /// <response code="409">Las fechas se solapan con otra reserva o no hay disponibilidad.</response>
        [Authorize(Roles = "User")]
        [HttpPost]
        [ProducesResponseType(typeof(ReservationResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ReservationResponseDTO>> PostReservation(ReservationRequestDTO reservationDTO)
        {
            try
            {
                var resertvationResponse = await _reservationService.CreateReservationAsync(reservationDTO);
                return Ok(resertvationResponse);
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

        /// <summary>Lista global de reservas con filtros opcionales (panel de administración).</summary>
        /// <param name="status">Estado de la reserva por el que filtrar.</param>
        /// <param name="propertyId">Filtrar por propiedad.</param>
        /// <param name="guestId">Filtrar por huésped.</param>
        /// <param name="from">Fecha de inicio mínima (inclusive).</param>
        /// <param name="to">Fecha de inicio máxima (inclusive).</param>
        /// <response code="200">Reservas que cumplen los filtros.</response>
        /// <response code="400">Algún filtro tiene un valor inválido.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("admin")]
        [ProducesResponseType(typeof(IEnumerable<ReservationResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReservationResponseDTO>>> GetAllReservationsForAdmin(
            [FromQuery] string? status,
            [FromQuery] int? propertyId,
            [FromQuery] int? guestId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            try
            {
                var reservations = await _reservationService.GetAllReservationsForAdminAsync(status, propertyId, guestId, from, to);
                return Ok(reservations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Cambia el estado de una reserva.</summary>
        /// <remarks>Permitido al dueño de la propiedad o a un administrador.</remarks>
        /// <param name="id">Identificador de la reserva.</param>
        /// <param name="dto">Nuevo estado a asignar.</param>
        /// <response code="204">Estado actualizado correctamente.</response>
        /// <response code="404">No existe la reserva.</response>
        /// <response code="409">La transición de estado no es válida en este momento.</response>
        [Authorize(Roles = "Owner,Admin")]
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeReservationStatus(int id, [FromBody] ChangeReservationStatusDTO dto)
        {
            try
            {
                await _reservationService.ChangeStatusAsync(
                    id,
                    dto
                );

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Obtiene el detalle de una reserva.</summary>
        /// <remarks>Accesible para el huésped dueño de la reserva, el dueño de la propiedad o un administrador.</remarks>
        /// <param name="reservationId">Identificador de la reserva.</param>
        /// <response code="200">Detalle de la reserva.</response>
        /// <response code="404">No existe la reserva.</response>
        [Authorize(Roles = "Owner, User, Admin")]
        [HttpGet("{reservationId}")]
        [ProducesResponseType(typeof(ReservationResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationResponseDTO>> getReservationById(int reservationId)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
                return Ok(reservation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
