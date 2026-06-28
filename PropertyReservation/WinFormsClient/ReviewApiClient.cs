using Application.DTOs.Review;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para interactuar con los endpoints de reseñas de la API.
public class ReviewApiClient
{
    private readonly HttpClient _httpClient;

    public ReviewApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene todas las reseñas del sistema (admin).
    public async Task<IEnumerable<ReviewResponseDTO>?> GetAllReviewsAsync()
    {
        var response = await _httpClient.GetAsync("api/review/all");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IEnumerable<ReviewResponseDTO>>();
        return null;
    }

    // Obtiene todas las reseñas de una propiedad.
    public async Task<IEnumerable<ReviewResponseDTO>?> GetPropertyReviewsAsync(int propertyId)
    {
        var response = await _httpClient.GetAsync($"api/review?propertyId={propertyId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<ReviewResponseDTO>>();
        }

        return null;
    }

    // Obtiene una reseña por su ID.
    public async Task<ReviewResponseDTO?> GetReviewByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/review/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ReviewResponseDTO>();
        }

        return null;
    }

    // Crea una nueva reseña.
    public async Task<ReviewResponseDTO?> CreateReviewAsync(ReviewRequestDTO reviewDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/review", reviewDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ReviewResponseDTO>();
        }

        return null;
    }

    // Actualiza una reseña existente.
    public async Task<bool> UpdateReviewAsync(int id, ReviewRequestDTO reviewDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/review/{id}", reviewDto);
        return response.IsSuccessStatusCode;
    }

    // Elimina una reseña.
    public async Task<bool> DeleteReviewAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/review/{id}");
        return response.IsSuccessStatusCode;
    }
}
