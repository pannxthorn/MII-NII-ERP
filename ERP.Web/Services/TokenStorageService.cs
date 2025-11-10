using System.Collections.Concurrent;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for storing and retrieving authentication token
    /// For Blazor Server, we use in-memory storage per connection
    /// </summary>
    public class TokenStorageService
    {
        private readonly ConcurrentDictionary<string, AuthData> _storage = new();
        private string? _currentConnectionId;

        private class AuthData
        {
            public string Token { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
        }

        public void SetConnectionId(string connectionId)
        {
            _currentConnectionId = connectionId;
        }

        /// <summary>
        /// Store authentication data
        /// </summary>
        public void SetAuthData(string token, string userId, string userName, DateTime expiresAt)
        {
            if (string.IsNullOrEmpty(_currentConnectionId))
                _currentConnectionId = Guid.NewGuid().ToString();

            _storage[_currentConnectionId] = new AuthData
            {
                Token = token,
                UserId = userId,
                UserName = userName,
                ExpiresAt = expiresAt
            };
        }

        /// <summary>
        /// Get stored token
        /// </summary>
        public string? GetToken()
        {
            if (string.IsNullOrEmpty(_currentConnectionId)) return null;

            if (_storage.TryGetValue(_currentConnectionId, out var data))
            {
                if (data.ExpiresAt <= DateTime.UtcNow)
                {
                    Clear();
                    return null;
                }
                return data.Token;
            }
            return null;
        }

        /// <summary>
        /// Get stored user ID
        /// </summary>
        public string? GetUserId()
        {
            if (string.IsNullOrEmpty(_currentConnectionId)) return null;
            return _storage.TryGetValue(_currentConnectionId, out var data) ? data.UserId : null;
        }

        /// <summary>
        /// Get stored username
        /// </summary>
        public string? GetUserName()
        {
            if (string.IsNullOrEmpty(_currentConnectionId)) return null;
            return _storage.TryGetValue(_currentConnectionId, out var data) ? data.UserName : null;
        }

        /// <summary>
        /// Check if user is authenticated
        /// </summary>
        public bool IsAuthenticated()
        {
            var token = GetToken();
            return !string.IsNullOrEmpty(token);
        }

        /// <summary>
        /// Clear all stored data
        /// </summary>
        public void Clear()
        {
            if (!string.IsNullOrEmpty(_currentConnectionId))
            {
                _storage.TryRemove(_currentConnectionId, out _);
            }
        }
    }
}
