using ERP.ApplicationDTO.users;
using ERP.Shared._base;
using ERP.Shared._base.BaseResponse;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for handling User API calls
    /// </summary>
    public class UserService : BaseApiService
    {
        public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger)
            : base(httpClientFactory, logger)
        {
        }

        /// <summary>
        /// Get all users with pagination
        /// </summary>
        public async Task<BaseResponse<PaginatedResponse<UserDTO>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await ExecuteGetAsync<PaginatedResponse<UserDTO>>(
                $"api/Users/GetAllUsers?pageNumber={pageNumber}&pageSize={pageSize}",
                unwrapDirectResponse: true);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<BaseResponse<UserDTO>> GetUserByIdAsync(string userId)
        {
            return await ExecuteGetAsync<UserDTO>($"api/Users/GetUserById?userId={userId}", unwrapDirectResponse: true);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        public async Task<BaseResponse<UserDTO>> CreateUserAsync(CreateUserDTO userDto)
        {
            return await ExecutePostAsync<CreateUserDTO, UserDTO>("api/Users/CreateUser", userDto);
        }
    }
}
