namespace ERP.Shared.models
{
    public class CurrentUserVM
    {
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
