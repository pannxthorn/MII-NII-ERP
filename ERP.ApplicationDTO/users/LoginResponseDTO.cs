namespace ERP.ApplicationDTO.users
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? CompanyId { get; set; }
        public string? BranchId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
