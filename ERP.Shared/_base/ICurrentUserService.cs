using ERP.Shared.models;

namespace ERP.Shared._base
{
    /// <summary>
    /// Service สำหรับดึงข้อมูล Current User จาก HttpContext
    /// </summary>
    public interface ICurrentUserService
    {
        CurrentUserVM? GetCurrentUser();
    }
}
