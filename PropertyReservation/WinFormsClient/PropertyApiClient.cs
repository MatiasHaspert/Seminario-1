using Application.DTOs.Property;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para interactuar con los endpoints de propiedades de la API.
public class PropertyApiClient
{
    private readonly HttpClient _httpClient;

    public PropertyApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene todas las propiedades. Si includeDeleted=true incluye las soft-deleted (solo Admin).
    public async Task<IEnumerable<PropertyListResponseDTO>?> GetPropertiesAsync(bool includeDeleted = false)
    {
        var url = "api/property" + (includeDeleted ? "?includeDeleted=true" : string.Empty);
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PropertyListResponseDTO>>();
        }

        return null;
    }

    // Obtiene una propiedad por su ID.
    public async Task<PropertyDetailsResponseDTO?> GetPropertyByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/property/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PropertyDetailsResponseDTO>();
        }

        return null;
    }

    public async Task<bool> DeletePropertyAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/property/{id}");
        if (response.IsSuccessStatusCode) return true;

        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase ?? "Error" : body;
        throw new HttpRequestException($"Error {(int)response.StatusCode}: {message}");
    }

    // Restaura una propiedad eliminada (solo Admin).
    public async Task<bool> RestorePropertyAsync(int id)
    {
        var response = await _httpClient.PostAsync($"api/property/{id}/restore", content: null);
        if (response.IsSuccessStatusCode) return true;

        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase ?? "Error" : body;
        throw new HttpRequestException($"Error {(int)response.StatusCode}: {message}");
    }
}
