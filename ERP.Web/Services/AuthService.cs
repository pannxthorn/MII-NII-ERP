using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using System.Net.Http.Json;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for handling authentication API calls
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IHttpClientFactory httpClientFactory, ILogger<AuthService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
        }

        /// <summary>
        /// Call Login API endpoint
        /// </summary>
        public async Task<BaseResponse<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", loginDto);

                // Log response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response Status: {response.StatusCode}");
                _logger.LogInformation($"API Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    // Check if response is empty
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return new BaseResponse<LoginResponseDTO>
                        {
                            IsSuccess = false,
                            Message = "API returned empty response"
                        };
                    }

                    // Try to parse JSON
                    try
                    {
                        var result = System.Text.Json.JsonSerializer.Deserialize<BaseResponse<LoginResponseDTO>>(
                            responseContent,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        return result ?? new BaseResponse<LoginResponseDTO>
                        {
                            IsSuccess = false,
                            Message = "Failed to parse response"
                        };
                    }
                    catch (System.Text.Json.JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON parsing error");
                        return new BaseResponse<LoginResponseDTO>
                        {
                            IsSuccess = false,
                            Message = $"Invalid JSON response: {jsonEx.Message}"
                        };
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new BaseResponse<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password"
                    };
                }
                else
                {
                    return new BaseResponse<LoginResponseDTO>
                    {
                        IsSuccess = false,
                        Message = $"Login failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error during login");
                return new BaseResponse<LoginResponseDTO>
                {
                    IsSuccess = false,
                    Message = "Cannot connect to the server. Please check your connection."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                return new BaseResponse<LoginResponseDTO>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        /// <summary>
        /// Logout user (clear stored data)
        /// </summary>
        public Task LogoutAsync()
        {
            // TODO: Clear stored token and user data
            return Task.CompletedTask;
        }
    }
}
