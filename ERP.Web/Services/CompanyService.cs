using ERP.ApplicationDTO.company;
using ERP.Shared._base;
using ERP.Shared._base.BaseResponse;

namespace ERP.Web.Services
{
    /// <summary>
    /// Service for handling Company API calls
    /// </summary>
    public class CompanyService : BaseApiService
    {
        public CompanyService(IHttpClientFactory httpClientFactory, ILogger<CompanyService> logger)
            : base(httpClientFactory, logger)
        {
        }

        /// <summary>
        /// Get all companies with pagination
        /// </summary>
        public async Task<BaseResponse<PaginatedResponse<CompanyDTO>>> GetAllCompanyAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await ExecuteGetAsync<PaginatedResponse<CompanyDTO>>(
                $"api/Company/GetAllCompany?pageNumber={pageNumber}&pageSize={pageSize}",
                unwrapDirectResponse: true);
        }

        /// <summary>
        /// Get company by ID
        /// </summary>
        public async Task<BaseResponse<CompanyDTO>> GetCompanyByIdAsync(string companyId)
        {
            return await ExecuteGetAsync<CompanyDTO>($"api/Company/GetCompanyById?companyId={companyId}");
        }

        /// <summary>
        /// Create new company
        /// </summary>
        public async Task<BaseResponse<CompanyDTO>> CreateCompanyAsync(CreateCompanyDTO companyDto)
        {
            return await ExecutePostAsync<CreateCompanyDTO, CompanyDTO>("api/Company/CreateCompany", companyDto);
        }
    }
}
