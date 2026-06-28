using Application.DTOs.Admin;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para los endpoints de estadísticas del Administrador.
public class AdminApiClient
{
    private readonly HttpClient _httpClient;

    public AdminApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene el resumen de estadísticas globales del sistema.
    public async Task<AdminStatsDTO?> GetStatsAsync()
    {
        var response = await _httpClient.GetAsync("api/admin/stats");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AdminStatsDTO>();

        return null;
    }
}
