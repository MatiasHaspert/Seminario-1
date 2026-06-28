using Application.DTOs.Payments;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    /// <summary>
    /// Comprobantes de pago: el huésped sube el comprobante de su reserva y puede eliminarlo
    /// mientras esté en revisión; el dueño (o el administrador) lo consulta y lo aprueba o rechaza.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentsService _paymentsService;

        public PaymentsController(PaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        /// <summary>Sube un comprobante de pago para una reserva.</summary>
        /// <remarks>
        /// Solo el huésped dueño de la reserva puede subir el comprobante. Se envía como
        /// <c>multipart/form-data</c> e incluye la reserva, el método de pago y el archivo.
        /// </remarks>
        /// <param name="paymentRequest">Reserva, método de pago y archivo del comprobante.</param>
        /// <response code="200">Comprobante registrado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">La reserva ya tiene un comprobante o no admite pago en este estado.</response>
        [HttpPost]
        [Authorize(Roles = "User")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(PaymentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PaymentResponseDTO>> CreatePayment([FromForm] PaymentRequestDTO paymentRequest)
        {
            try
            {
                var paymentResponseDTO = await _paymentsService.CreatePayment(paymentRequest);
                return Ok(paymentResponseDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>Descarga el archivo del comprobante de un pago.</summary>
        /// <remarks>Accesible para el huésped dueño del pago, el dueño de la propiedad o un administrador.</remarks>
        /// <param name="paymentId">Identificador del pago.</param>
        /// <response code="200">Archivo binario del comprobante.</response>
        /// <response code="404">No existe el pago o el archivo del comprobante.</response>
        [HttpGet("{paymentId}/proof")]
        [Authorize(Roles = "Owner, User, Admin")]
        [Produces("application/octet-stream", "image/png", "image/jpeg")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProof(Guid paymentId)
        {
            try
            {
                var result = await _paymentsService.GetPaymentProofUrl(paymentId);
                return PhysicalFile(result.ProofPath, result.ContentType);
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
                return Conflict(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Lista los comprobantes pendientes de revisión.</summary>
        /// <remarks>El dueño ve solo los de sus propiedades; el administrador, todos.</remarks>
        /// <response code="200">Comprobantes en revisión.</response>
        [HttpGet("underReview")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(typeof(List<PendingPaymentListDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PendingPaymentListDTO>>> GetPaymentsUnderReview()
        {
            return Ok(await _paymentsService.GetPaymentsUnderReview());
        }

        /// <summary>Aprueba o rechaza un comprobante de pago.</summary>
        /// <remarks>Permitido al dueño de la propiedad o a un administrador.</remarks>
        /// <param name="paymentId">Identificador del pago.</param>
        /// <param name="status">Nuevo estado del comprobante (aprobado o rechazado).</param>
        /// <response code="204">Estado del comprobante actualizado.</response>
        /// <response code="404">No existe el pago.</response>
        /// <response code="409">El comprobante no admite el cambio de estado solicitado.</response>
        [HttpPatch("{paymentId}/status")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeStatusPayment(Guid paymentId, [FromQuery] ChangePaymentStatusDTO status)
        {
            try
            {
                await _paymentsService.ChangeStatusPayment(paymentId, status);
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
                return Conflict(ex.Message);
            }
        }

        /// <summary>Elimina un comprobante que está en revisión.</summary>
        /// <remarks>Solo el huésped que lo subió puede eliminarlo, y únicamente mientras siga en revisión.</remarks>
        /// <param name="paymentId">Identificador del pago.</param>
        /// <response code="204">Comprobante eliminado correctamente.</response>
        /// <response code="404">No existe el pago.</response>
        /// <response code="409">El comprobante ya fue revisado y no puede eliminarse.</response>
        [HttpDelete("{paymentId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeletePayment(Guid paymentId)
        {
            try
            {
                await _paymentsService.DeletePayment(paymentId);
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
                return Conflict(ex.Message);
            }
        }

    }

}
