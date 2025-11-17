using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for handling User API calls
    /// </summary>
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly TokenStorageService _tokenStorage;

        public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger, TokenStorageService tokenStorage)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
            _tokenStorage = tokenStorage;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<BaseResponse<List<UserDTO>>> GetAllUsersAsync()
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync("api/Users/GetAllUsers");

                // Log response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response Status: {response.StatusCode}");
                _logger.LogInformation($"API Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    // Check if response is empty
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return new BaseResponse<List<UserDTO>>
                        {
                            IsSuccess = false,
                            Message = "API returned empty response"
                        };
                    }

                    // Try to parse JSON - API returns List<UserDTO> directly, not wrapped in BaseResponse
                    try
                    {
                        var data = System.Text.Json.JsonSerializer.Deserialize<List<UserDTO>>(
                            responseContent,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        return new BaseResponse<List<UserDTO>>
                        {
                            IsSuccess = true,
                            Data = data,
                            Message = "Success"
                        };
                    }
                    catch (System.Text.Json.JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON parsing error");
                        return new BaseResponse<List<UserDTO>>
                        {
                            IsSuccess = false,
                            Message = $"Invalid JSON response: {jsonEx.Message}"
                        };
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new BaseResponse<List<UserDTO>>
                    {
                        IsSuccess = false,
                        Message = "Unauthorized. Please login again."
                    };
                }
                else
                {
                    return new BaseResponse<List<UserDTO>>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error during get all users");
                return new BaseResponse<List<UserDTO>>
                {
                    IsSuccess = false,
                    Message = "Cannot connect to the server. Please check your connection."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during get all users");
                return new BaseResponse<List<UserDTO>>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<BaseResponse<UserDTO>> GetUserByIdAsync(string userId)
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync($"api/Users/GetUserById?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var data = System.Text.Json.JsonSerializer.Deserialize<UserDTO>(
                        responseContent,
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return new BaseResponse<UserDTO>
                    {
                        IsSuccess = true,
                        Data = data,
                        Message = "Success"
                    };
                }
                else
                {
                    return new BaseResponse<UserDTO>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during get user by ID");
                return new BaseResponse<UserDTO>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        public async Task<BaseResponse<UserDTO>> CreateUserAsync(CreateUserDTO userDto)
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.PostAsJsonAsync("api/Users/CreateUser", userDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<UserDTO>>(
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return result ?? new BaseResponse<UserDTO>
                    {
                        IsSuccess = false,
                        Message = "Failed to parse response"
                    };
                }
                else
                {
                    return new BaseResponse<UserDTO>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during create user");
                return new BaseResponse<UserDTO>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }
    }
}
