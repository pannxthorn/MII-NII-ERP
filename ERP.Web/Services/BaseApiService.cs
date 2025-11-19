using ERP.Shared._base.BaseResponse;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ERP.Web.Services
{
    /// <summary>
    /// Base service class for all API services with common HTTP operations and error handling
    /// </summary>
    public abstract class BaseApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected BaseApiService(IHttpClientFactory httpClientFactory, ILogger logger, string clientName = "ApiClient")
        {
            _httpClient = httpClientFactory.CreateClient(clientName);
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Execute GET request and return response
        /// </summary>
        /// <typeparam name="TResult">The type of data expected in the response</typeparam>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="unwrapDirectResponse">If true, API returns data directly (not wrapped in BaseResponse). Default: false</param>
        protected async Task<BaseResponse<TResult>> ExecuteGetAsync<TResult>(
            string endpoint,
            bool unwrapDirectResponse = false)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                return await ProcessHttpResponseAsync<TResult>(response, unwrapDirectResponse, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return HandleHttpRequestException<TResult>(ex, endpoint);
            }
            catch (Exception ex)
            {
                return HandleUnexpectedException<TResult>(ex, endpoint);
            }
        }

        /// <summary>
        /// Execute POST request and return response
        /// </summary>
        /// <typeparam name="TRequest">The type of data to send</typeparam>
        /// <typeparam name="TResult">The type of data expected in the response</typeparam>
        /// <param name="endpoint">API endpoint to call</param>
        /// <param name="data">Data to send in the request body</param>
        protected async Task<BaseResponse<TResult>> ExecutePostAsync<TRequest, TResult>(
            string endpoint,
            TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                return await ProcessHttpResponseAsync<TResult>(response, false, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return HandleHttpRequestException<TResult>(ex, endpoint);
            }
            catch (Exception ex)
            {
                return HandleUnexpectedException<TResult>(ex, endpoint);
            }
        }

        /// <summary>
        /// Execute PUT request and return response
        /// </summary>
        protected async Task<BaseResponse<TResult>> ExecutePutAsync<TRequest, TResult>(
            string endpoint,
            TRequest data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data);
                return await ProcessHttpResponseAsync<TResult>(response, false, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return HandleHttpRequestException<TResult>(ex, endpoint);
            }
            catch (Exception ex)
            {
                return HandleUnexpectedException<TResult>(ex, endpoint);
            }
        }

        /// <summary>
        /// Execute DELETE request and return response
        /// </summary>
        protected async Task<BaseResponse<TResult>> ExecuteDeleteAsync<TResult>(
            string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return await ProcessHttpResponseAsync<TResult>(response, false, endpoint);
            }
            catch (HttpRequestException ex)
            {
                return HandleHttpRequestException<TResult>(ex, endpoint);
            }
            catch (Exception ex)
            {
                return HandleUnexpectedException<TResult>(ex, endpoint);
            }
        }

        /// <summary>
        /// Process HTTP response and handle different status codes
        /// </summary>
        private async Task<BaseResponse<TResult>> ProcessHttpResponseAsync<TResult>(
            HttpResponseMessage response,
            bool unwrapDirectResponse,
            string endpoint)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API Response [{endpoint}] Status: {response.StatusCode}");
            _logger.LogDebug($"API Response [{endpoint}] Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                return await DeserializeResponseAsync<TResult>(responseContent, unwrapDirectResponse, endpoint);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = "Unauthorized. Please login again."
                };
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = "You don't have permission to access this resource."
                };
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = "The requested resource was not found."
                };
            }
            else
            {
                return new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = $"Request failed with status code: {response.StatusCode}"
                };
            }
        }

        /// <summary>
        /// Deserialize JSON response to typed object
        /// </summary>
        private Task<BaseResponse<TResult>> DeserializeResponseAsync<TResult>(
            string responseContent,
            bool unwrapDirectResponse,
            string endpoint)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                _logger.LogWarning($"API returned empty response for endpoint: {endpoint}");
                return Task.FromResult(new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = "API returned empty response"
                });
            }

            try
            {
                if (unwrapDirectResponse)
                {
                    // API returns data directly (not wrapped in BaseResponse)
                    var data = JsonSerializer.Deserialize<TResult>(responseContent, _jsonOptions);
                    return Task.FromResult(new BaseResponse<TResult>
                    {
                        IsSuccess = true,
                        Data = data,
                        Message = "Success"
                    });
                }
                else
                {
                    // API returns BaseResponse<T>
                    var result = JsonSerializer.Deserialize<BaseResponse<TResult>>(responseContent, _jsonOptions);
                    return Task.FromResult(result ?? new BaseResponse<TResult>
                    {
                        IsSuccess = false,
                        Message = "Failed to parse response"
                    });
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, $"JSON parsing error for endpoint: {endpoint}");
                return Task.FromResult(new BaseResponse<TResult>
                {
                    IsSuccess = false,
                    Message = $"Invalid JSON response: {jsonEx.Message}"
                });
            }
        }

        /// <summary>
        /// Handle HTTP request exceptions (network errors, timeout, etc.)
        /// </summary>
        private BaseResponse<TResult> HandleHttpRequestException<TResult>(HttpRequestException ex, string endpoint)
        {
            _logger.LogError(ex, $"HTTP request error for endpoint: {endpoint}");
            return new BaseResponse<TResult>
            {
                IsSuccess = false,
                Message = "Cannot connect to the server. Please check your connection."
            };
        }

        /// <summary>
        /// Handle unexpected exceptions
        /// </summary>
        private BaseResponse<TResult> HandleUnexpectedException<TResult>(Exception ex, string endpoint)
        {
            _logger.LogError(ex, $"Unexpected error for endpoint: {endpoint}");
            return new BaseResponse<TResult>
            {
                IsSuccess = false,
                Message = "An unexpected error occurred. Please try again."
            };
        }
    }
}
