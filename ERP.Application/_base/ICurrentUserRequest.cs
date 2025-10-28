using ERP.ApplicationDTO.users;

namespace ERP.Application._base
{
    /// <summary>
    /// Interface สำหรับ Command/Query ที่ต้องการ CurrentUser
    /// </summary>
    public interface ICurrentUserRequest
    {
        CurrentUserVM? CurrentUser { get; set; }
    }
}
