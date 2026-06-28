using Application.DTOs.Amenity;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para interactuar con los endpoints de amenidades de la API.
public class AmenityApiClient
{
    private readonly HttpClient _httpClient;

    public AmenityApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene todas las amenidades.
    public async Task<IEnumerable<AmenityResponseDTO>?> GetAllAmenitiesAsync()
    {
        var response = await _httpClient.GetAsync("api/amenity");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<AmenityResponseDTO>>();
        }

        return null;
    }

    // Crea una nueva amenidad.
    public async Task<AmenityResponseDTO?> CreateAmenityAsync(AmenityRequestDTO amenityDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/amenity", amenityDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AmenityResponseDTO>();
        }

        return null;
    }

    // Actualiza una amenidad existente.
    public async Task<bool> UpdateAmenityAsync(int id, AmenityRequestDTO amenityDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/amenity/{id}", amenityDto);
        if (response.IsSuccessStatusCode) return true;

        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase ?? "Error" : body;
        throw new HttpRequestException($"Error {(int)response.StatusCode}: {message}");
    }

    // Elimina una amenidad.
    public async Task<bool> DeleteAmenityAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/amenity/{id}");
        return response.IsSuccessStatusCode;
    }
}
