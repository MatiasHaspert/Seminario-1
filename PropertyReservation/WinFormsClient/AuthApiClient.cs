using Application.DTOs.User;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para interactuar con los endpoints de autenticación de la API.
public class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Inicia sesión con las credenciales proporcionadas.
    public async Task<LoginResponseDTO?> LoginAsync(UserLoginDTO loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (loginResponse != null)
            {
                SessionManager.JwtToken = loginResponse.Token;
            }
            return loginResponse;
        }

        return null;
    }

    // Registra un nuevo usuario.
    public async Task<LoginResponseDTO?> RegisterAsync(UserRegisterDTO registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            if (loginResponse != null)
            {
                SessionManager.JwtToken = loginResponse.Token;
            }
            return loginResponse;
        }

        return null;
    }

    // Cierra la sesión del usuario actual.
    public void Logout()
    {
        SessionManager.Logout();
    }
}
