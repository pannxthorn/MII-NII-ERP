using ERP.ApplicationDTO.users;
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
        /// Get all users
        /// </summary>
        public async Task<BaseResponse<List<UserDTO>>> GetAllUsersAsync()
        {
            return await ExecuteGetAsync<List<UserDTO>>("api/Users/GetAllUsers", unwrapDirectResponse: true);
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
