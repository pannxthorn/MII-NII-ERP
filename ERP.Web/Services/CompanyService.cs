using ERP.ApplicationDTO.company;
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
        /// Get all companies
        /// </summary>
        public async Task<BaseResponse<List<CompanyDTO>>> GetAllCompanyAsync()
        {
            return await ExecuteGetAsync<List<CompanyDTO>>("api/Company/GetAllCompany", unwrapDirectResponse: true);
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
