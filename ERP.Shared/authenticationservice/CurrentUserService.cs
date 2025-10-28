using ERP.Shared._base;
using ERP.Shared.models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ERP.Shared.authenticationservice
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUserVM? GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
                return null;

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            var userNameClaim = httpContext.User.FindFirst(ClaimTypes.Name);
            var companyIdClaim = httpContext.User.FindFirst("CompanyId");
            var branchIdClaim = httpContext.User.FindFirst("BranchId");

            return new CurrentUserVM
            {
                UserId = int.Parse(userIdClaim.Value),
                UserName = userNameClaim?.Value ?? string.Empty,
                CompanyId = companyIdClaim != null ? int.Parse(companyIdClaim.Value) : null,
                BranchId = branchIdClaim != null ? int.Parse(branchIdClaim.Value) : null
            };
        }
    }
}
