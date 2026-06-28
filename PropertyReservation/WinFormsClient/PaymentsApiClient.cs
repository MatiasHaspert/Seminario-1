using Application.DTOs.Payments;
using System.Net.Http.Json;

namespace WinFormsClient;

// Cliente para los endpoints de pagos. Para el WinFormsApp se asume rol Admin.
public class PaymentsApiClient
{
    private readonly HttpClient _httpClient;

    public PaymentsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Pagos en estado UnderReview pendientes de aprobación.
    public async Task<List<PendingPaymentListDTO>?> GetPaymentsUnderReviewAsync()
    {
        var response = await _httpClient.GetAsync("api/payments/underReview");
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<List<PendingPaymentListDTO>>();

        return null;
    }

    // Descarga el comprobante de pago como bytes + content type.
    public async Task<(byte[] Content, string ContentType)?> GetPaymentProofAsync(Guid paymentId)
    {
        var response = await _httpClient.GetAsync($"api/payments/{paymentId}/proof");
        if (!response.IsSuccessStatusCode) return null;

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        return (bytes, contentType);
    }

    // Cambia el estado del pago (Approved / Rejected).
    public async Task<bool> ChangePaymentStatusAsync(Guid paymentId, ChangePaymentStatusDTO dto)
    {
        // El endpoint actual toma el estado por query string ([FromQuery]).
        var response = await _httpClient.PatchAsync(
            $"api/payments/{paymentId}/status?paymentStatus={dto.PaymentStatus}",
            content: null);

        if (response.IsSuccessStatusCode) return true;

        var body = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(body) ? response.ReasonPhrase ?? "Error" : body;
        throw new HttpRequestException($"Error {(int)response.StatusCode}: {message}");
    }
}
