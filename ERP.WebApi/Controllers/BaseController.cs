using ERP.Shared.models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private CurrentUserVM? _currentUser;

        /// <summary>
        /// ดึงข้อมูล current user จาก JWT claims (with lazy initialization + caching)
        /// Throw UnauthorizedAccessException ถ้าไม่มี authentication
        /// </summary>
        /// <returns>CurrentUserVM object (ไม่เป็น null)</returns>
        /// <exception cref="UnauthorizedAccessException">ถ้า user ไม่ได้ login</exception>
        protected CurrentUserVM GetCurrentUser()
        {
            // ถ้า cache แล้ว ให้ return ทันที
            if (_currentUser != null)
                return _currentUser;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User is not authenticated");

            var userNameClaim = User.FindFirst(ClaimTypes.Name);
            var companyIdClaim = User.FindFirst("CompanyId");
            var branchIdClaim = User.FindFirst("BranchId");

            // Cache ไว้ใน _currentUser
            _currentUser = new CurrentUserVM
            {
                UserId = int.Parse(userIdClaim.Value),
                UserName = userNameClaim?.Value ?? string.Empty,
                CompanyId = companyIdClaim != null ? int.Parse(companyIdClaim.Value) : null,
                BranchId = branchIdClaim != null ? int.Parse(branchIdClaim.Value) : null
            };

            return _currentUser;
        }
    }
}
