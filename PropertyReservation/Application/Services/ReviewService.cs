using AutoMapper;
using Domain.Entities;
using Application.DTOs.Review;
using Infrastructure.Repositories;

namespace Application.Services
{
    // Gestiona la creación, consulta y moderación de reseñas de propiedades.
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly PropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly CurrentUserService _currentUserService;

        public ReviewService(
            ReviewRepository reviewRepository,
            PropertyRepository propertyRepository,
            IMapper mapper,
            CurrentUserService currentUserService
        )
        {
            _reviewRepository = reviewRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ReviewResponseDTO> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new KeyNotFoundException("Reseña no encontrado.");
            }
            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllOrderedByDateAsync();
            return reviews.Select(r => _mapper.Map<ReviewResponseDTO>(r)).ToList();
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetPropertyReviewsAsync(int propertyId)
        {
            var propertyExists = await _propertyRepository.ExistsAsync(propertyId);
            if (!propertyExists)
            {
                throw new KeyNotFoundException("Propiedad no encontrada.");
            }
            var reviews = await _reviewRepository.GetByPropertyIdAsync(propertyId);
            return reviews.Select(r => _mapper.Map<ReviewResponseDTO>(r)).ToList();
        }

        public async Task<ReviewResponseDTO> CreateReviewAsync(ReviewRequestDTO reviewRequestDTO)
        {
            var userId = _currentUserService.UserId;

            var propertyExists = await _propertyRepository.ExistsAsync(reviewRequestDTO.PropertyId);
            if (!propertyExists)
                throw new KeyNotFoundException("Propiedad no encontrada.");

            // Verificar si el usuario ya ha escrito una reseña para esta propiedad
            var existingReview = await _reviewRepository.GetByUserAndPropertyAsync(userId, reviewRequestDTO.PropertyId);
            if (existingReview != null)
                throw new InvalidOperationException("Ya has escrito una reseña para esta propiedad.");

            // Verificar que la reserva haya sido completada antes de permitir escribir una reseña
            var hasCompletedReservation = await _reviewRepository.HasUserCompletedReservationAsync(userId, reviewRequestDTO.PropertyId);
            if (!hasCompletedReservation)
                throw new InvalidOperationException("Solo puedes crear una reseña si has completado una reserva para esta propiedad.");

            var review = _mapper.Map<Review>(reviewRequestDTO);
            review.UserId = userId;

            await _reviewRepository.AddAsync(review);
            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task UpdateReviewAsync(int reviewId, ReviewRequestDTO reviewRequestDTO)
        {
            var userId = _currentUserService.UserId;

            var propertyExists = await _propertyRepository.ExistsAsync(reviewRequestDTO.PropertyId);
            if (!propertyExists)
                throw new KeyNotFoundException("Propiedad no encontrada.");

            var review = await _reviewRepository.FindByIdAsync(reviewId);
            if (review == null)
                throw new KeyNotFoundException("Reseña no encontrada.");

            // Verificar si el usuario actual es el autor de la reseña
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("No tienes permiso para actualizar esta reseña.");

            review.Rating = reviewRequestDTO.Rating;
            review.Comment = reviewRequestDTO.Comment;

            await _reviewRepository.UpdateAsync(review);
        }

        // Eliminación de una reseña inapropiada por parte del Administrador.
        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _reviewRepository.FindByIdAsync(reviewId);
            if (review == null)
                throw new KeyNotFoundException("Reseña no encontrada.");

            await _reviewRepository.DeleteAsync(review);
        }

    }
}
