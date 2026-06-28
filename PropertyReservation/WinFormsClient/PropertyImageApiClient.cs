using Application.DTOs.PropertyImage;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para interactuar con los endpoints de imágenes de propiedades de la API.
public class PropertyImageApiClient
{
    private readonly HttpClient _httpClient;

    public PropertyImageApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene todas las imágenes de una propiedad.
    public async Task<IEnumerable<PropertyImageResponseDTO>?> GetImagesByPropertyAsync(int propertyId)
    {
        var response = await _httpClient.GetAsync($"api/propertyimage/{propertyId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PropertyImageResponseDTO>>();
        }

        return null;
    }

    // Sube imágenes para una propiedad.
    public async Task<IEnumerable<PropertyImageResponseDTO>?> UploadImagesAsync(int propertyId, IEnumerable<Stream> imageStreams, IEnumerable<string> fileNames)
    {
        using var content = new MultipartFormDataContent();

        var streamList = imageStreams.ToList();
        var nameList = fileNames.ToList();

        for (int i = 0; i < streamList.Count && i < nameList.Count; i++)
        {
            var streamContent = new StreamContent(streamList[i]);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            content.Add(streamContent, "files", nameList[i]);
        }

        var response = await _httpClient.PostAsync($"api/propertyimage/{propertyId}", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<PropertyImageResponseDTO>>();
        }

        return null;
    }

    // Elimina una imagen de una propiedad.
    public async Task<bool> DeleteImageAsync(int imageId)
    {
        var response = await _httpClient.DeleteAsync($"api/propertyimage/{imageId}");
        return response.IsSuccessStatusCode;
    }
}
