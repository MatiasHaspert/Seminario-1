using Application.DTOs.User;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para los endpoints de gestión de usuarios. Todos requieren rol Admin.
public class UsersApiClient
{
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Lista usuarios con filtros opcionales por email, rol y estado.
    public async Task<IEnumerable<UserListDTO>?> GetUsersAsync(string? email = null, string? role = null, bool? isActive = null)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(email)) parts.Add($"email={Uri.EscapeDataString(email)}");
        if (!string.IsNullOrWhiteSpace(role)) parts.Add($"role={Uri.EscapeDataString(role)}");
        if (isActive.HasValue) parts.Add($"isActive={(isActive.Value ? "true" : "false")}");

        var url = "api/users" + (parts.Count > 0 ? "?" + string.Join("&", parts) : string.Empty);

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserListDTO>>();

        return null;
    }

    public async Task<UserDetailDTO?> GetUserByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/users/{id}");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<UserDetailDTO>();

        return null;
    }

    public async Task<bool> UpdateUserRoleAsync(int id, UpdateUserRoleDTO dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{id}/role", dto);
        if (response.IsSuccessStatusCode) return true;
        throw await BuildExceptionAsync(response);
    }

    public async Task<bool> UpdateUserStatusAsync(int id, UpdateUserStatusDTO dto)
    {
        var response = await _httpClient.PatchAsJsonAsync($"api/users/{id}/status", dto);
        if (response.IsSuccessStatusCode) return true;
        throw await BuildExceptionAsync(response);
    }

    private static async Task<HttpRequestException> BuildExceptionAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase ?? "Error" : body;
        return new HttpRequestException($"Error {(int)response.StatusCode}: {message}");
    }
}
