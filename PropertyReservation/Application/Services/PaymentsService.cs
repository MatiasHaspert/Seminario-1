using Application.DTOs.Payments;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Gestiona la carga, consulta y aprobación de comprobantes de pago.
    public class PaymentsService
    {
        private readonly IWebHostEnvironment _env;
        private readonly PaymentsRepository _paymentsRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly CurrentUserService _currentUserService;

        public PaymentsService(
            IWebHostEnvironment environment,
            PaymentsRepository paymentsRepository,
            ReservationRepository reservationRepository,
            IMapper mapper,
            CurrentUserService currentUserService)
        {
            _env = environment;
            _mapper = mapper;
            _paymentsRepository = paymentsRepository;
            _reservationRepository = reservationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PaymentResponseDTO> CreatePayment(PaymentRequestDTO dto)
        {
            var userId = _currentUserService.UserId;

            if (dto.file == null || dto.file.Length == 0)
                throw new ArgumentException("El comprobante es obligatorio.");

            // Validar que la reserva no tenga un pago aprobado o bajo revision
            if (await _paymentsRepository.HasApprovedOrUnderReviewPaymentAsync(dto.reservationId))
                throw new InvalidOperationException("La reserva ya tiene un pago aprobado o bajo revisión.");

            // Validar existencia de la reserva
            var reservation = await _reservationRepository
                .GetByIdWithPropertyAsync(dto.reservationId);

            if (reservation == null)
                throw new KeyNotFoundException("Reserva no encontrada.");

            // Validar que la reserva pertenezca al usuario
            if (reservation.GuestId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para crear un pago para esta reserva.");

            // Validamos estado
            reservation.UploadPayment();

            // Validar archivo
            ValidateFile(dto.file);

            // Crear pago
            var payment = new Payment(reservation, dto.PaymentMethod);

            await _paymentsRepository.AddAsync(payment);
            await _paymentsRepository.SaveChangesAsync(); // Persistimos el pago para obtener el Id.

            var extension = Path.GetExtension(dto.file.FileName);
            var filePath = BuildPaymentPath(payment.Id, extension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.file.CopyToAsync(stream);
            }

            payment.ProofPath = filePath;

            await _paymentsRepository.SaveChangesAsync();

            return _mapper.Map<PaymentResponseDTO>(payment);
        }


        public async Task<(string ProofPath, string ContentType)> GetPaymentProofUrl(Guid paymentId)
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsInRole("Admin");

            // Validar pago
            var payment = await _paymentsRepository.GetByIdWithReservationAndPropertyAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"El pago con ID:{paymentId} no existe.");

            // El Admin puede ver cualquier comprobante; el owner/guest sólo los propios.
            var isGuest = payment.Reservation.GuestId == userId;
            var isOwner = payment.Reservation.Property.OwnerId == userId;
            if (!isAdmin && !isOwner && !isGuest)
                throw new UnauthorizedAccessException("No tiene permiso para ver el comprobante de este pago.");

            // Validar comprobante
            if (string.IsNullOrEmpty(payment.ProofPath))
                throw new InvalidOperationException("El pago no tiene un comprobante asociado.");

            // Validar archivo fisico
            if (!File.Exists(payment.ProofPath))
                throw new FileNotFoundException("El archivo del comprobante no fue encontrado en el servidor.");

            // Content type dinamico
            var contentType = GetContentType(payment.ProofPath);

            // Retornar path
            return (payment.ProofPath, contentType);
        }

        private static string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();

            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }

        public async Task<List<PendingPaymentListDTO>> GetPaymentsUnderReview()
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsInRole("Admin");

            // El Admin ve todos los pagos bajo revisión; el Owner sólo los de sus propiedades.
            var payments = isAdmin
                ? await _paymentsRepository.GetAllUnderReviewPaymentsAsync()
                : await _paymentsRepository.GetUnderReviewPaymentsByOwnerIdAsync(userId);

            return _mapper.Map<List<PendingPaymentListDTO>>(payments);
        }

        public async Task ChangeStatusPayment(Guid paymentId, ChangePaymentStatusDTO status)
        {
            var userId = _currentUserService.UserId;
            var isAdmin = _currentUserService.IsInRole("Admin");

            // Validar pago
            var payment = await _paymentsRepository.GetByIdWithReservationAndPropertyAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"El pago con ID:{paymentId} no existe.");

            // El Admin puede operar sobre cualquier pago; el Owner sólo sobre los de sus propiedades.
            if (!isAdmin && payment.Reservation.Property.OwnerId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para cambiar el estado de este pago.");

            if (isAdmin && payment.Method != PaymentMethod.CreditCard && payment.Method != PaymentMethod.DebitCard)
                throw new InvalidOperationException("Solo se pueden aprobar o rechazar pagos realizados con tarjeta de crédito o débito.");

            switch (status.PaymentStatus)
            {
                case PaymentStatus.Approved:
                    payment.Approve(payment.Reservation);
                    break;

                case PaymentStatus.Rejected:
                    payment.Reject(payment.Reservation);
                    break;

                default:
                    throw new InvalidOperationException("Estado inválido.");
            }

            // Persistir cambio
            await _paymentsRepository.SaveChangesAsync();
        }

        public async Task DeletePayment(Guid paymentId)
        {
            var userId = _currentUserService.UserId;

            // Validar pago
            var payment = await _paymentsRepository.GetByIdWithReservationAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException($"El pago con ID:{paymentId} no existe.");

            // Validar que la reserva pertenezca al usuario
            if (payment.Reservation.GuestId != userId)
                throw new UnauthorizedAccessException("No tiene permiso para eliminar este pago.");

            // Validar que el pago este en estado pendiente
            if (payment.Status != PaymentStatus.UnderReview)
                throw new InvalidOperationException("Solo se pueden eliminar pagos en estado pendiente.");

            // Eliminar archivo fisico
            if (!string.IsNullOrEmpty(payment.ProofPath) && File.Exists(payment.ProofPath))
            {
                File.Delete(payment.ProofPath);
            }

            // Eliminar pago
            await _paymentsRepository.DeleteAsync(payment);
        }

        private void ValidateFile(IFormFile file)
        {
            var allowedTypes = new[]
            {
                "application/pdf",
                "image/jpeg",
                "image/png"
            };

            if (!allowedTypes.Contains(file.ContentType))
                throw new InvalidOperationException("Tipo de archivo no permitido.");

            var extension = Path.GetExtension(file.FileName).ToLower();

            var allowedExtensions = new[]
            {
                ".pdf", ".jpg", ".jpeg", ".png"
            };

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Extensión no permitida.");
        }

        private string BuildPaymentPath(Guid paymentId, string extension)
        {
            var storagePath = Path.Combine(
                _env.ContentRootPath,
                "Storage",
                "Payments"
            );

            Directory.CreateDirectory(storagePath);

            return Path.Combine(storagePath, $"{paymentId}{extension}");
        }

    }
}
