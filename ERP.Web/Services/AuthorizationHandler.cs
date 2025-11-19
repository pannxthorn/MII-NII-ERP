using System.Net.Http.Headers;

namespace ERP.Web.Services
{
    /// <summary>
    /// DelegatingHandler that automatically adds JWT token to all HTTP requests
    /// </summary>
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly TokenStorageService _tokenStorage;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(TokenStorageService tokenStorage, ILogger<AuthorizationHandler> logger)
        {
            _tokenStorage = tokenStorage;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Get token from storage
            var token = _tokenStorage.GetToken();

            // Add Authorization header if token exists
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogDebug("Authorization token added to request: {Method} {Uri}", request.Method, request.RequestUri);
            }
            else
            {
                _logger.LogWarning("No token available for request: {Method} {Uri}", request.Method, request.RequestUri);
            }

            // Continue with the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
