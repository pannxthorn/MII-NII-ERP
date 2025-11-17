using ERP.ApplicationDTO.company;
using ERP.Shared._base.BaseResponse;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for handling Company API calls
    /// </summary>
    public class CompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyService> _logger;
        private readonly TokenStorageService _tokenStorage;

        public CompanyService(HttpClient httpClient, ILogger<CompanyService> logger, TokenStorageService tokenStorage)
        {
            _httpClient = httpClient;
            _logger = logger;
            _tokenStorage = tokenStorage;
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        public async Task<BaseResponse<List<CompanyDTO>>> GetAllCompanyAsync()
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync("api/Company/GetAllCompany");

                // Log response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response Status: {response.StatusCode}");
                _logger.LogInformation($"API Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    // Check if response is empty
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return new BaseResponse<List<CompanyDTO>>
                        {
                            IsSuccess = false,
                            Message = "API returned empty response"
                        };
                    }

                    // Try to parse JSON - API returns List<CompanyDTO> directly, not wrapped in BaseResponse
                    try
                    {
                        var data = System.Text.Json.JsonSerializer.Deserialize<List<CompanyDTO>>(
                            responseContent,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        return new BaseResponse<List<CompanyDTO>>
                        {
                            IsSuccess = true,
                            Data = data,
                            Message = "Success"
                        };
                    }
                    catch (System.Text.Json.JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON parsing error");
                        return new BaseResponse<List<CompanyDTO>>
                        {
                            IsSuccess = false,
                            Message = $"Invalid JSON response: {jsonEx.Message}"
                        };
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new BaseResponse<List<CompanyDTO>>
                    {
                        IsSuccess = false,
                        Message = "Unauthorized. Please login again."
                    };
                }
                else
                {
                    return new BaseResponse<List<CompanyDTO>>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error during get all company");
                return new BaseResponse<List<CompanyDTO>>
                {
                    IsSuccess = false,
                    Message = "Cannot connect to the server. Please check your connection."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during get all company");
                return new BaseResponse<List<CompanyDTO>>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        /// <summary>
        /// Get company by ID
        /// </summary>
        public async Task<BaseResponse<CompanyDTO>> GetCompanyByIdAsync(string companyId)
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetAsync($"api/Company/GetCompanyById?companyId={companyId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<CompanyDTO>>(
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return result ?? new BaseResponse<CompanyDTO>
                    {
                        IsSuccess = false,
                        Message = "Failed to parse response"
                    };
                }
                else
                {
                    return new BaseResponse<CompanyDTO>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during get company by ID");
                return new BaseResponse<CompanyDTO>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        /// <summary>
        /// Create new company
        /// </summary>
        public async Task<BaseResponse<CompanyDTO>> CreateCompanyAsync(CreateCompanyDTO companyDto)
        {
            try
            {
                // Add Authorization token
                var token = _tokenStorage.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.PostAsJsonAsync("api/Company/CreateCompany", companyDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<CompanyDTO>>(
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return result ?? new BaseResponse<CompanyDTO>
                    {
                        IsSuccess = false,
                        Message = "Failed to parse response"
                    };
                }
                else
                {
                    return new BaseResponse<CompanyDTO>
                    {
                        IsSuccess = false,
                        Message = $"Request failed with status code: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during create company");
                return new BaseResponse<CompanyDTO>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }
    }
}
