using System.Net.Http.Headers;

namespace WinFormsClient;

// Delegating handler que intercepta cada petición HTTP saliente y añade
// el token JWT de autenticación si está disponible.
public class AuthenticationHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(SessionManager.JwtToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
