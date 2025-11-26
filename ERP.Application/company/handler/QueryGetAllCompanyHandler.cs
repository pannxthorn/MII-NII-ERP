using AutoMapper;
using ERP.Application.company.queries;
using ERP.Application.Extensions;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.Domain.entities;
using ERP.Shared._base;
using ERP.Shared.encryptservice;
using ERP.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP.Application.company.handler
{
    public class QueryGetAllCompanyHandler : IRequestHandler<QueryGetAllCompany, PaginatedResponse<CompanyDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly ILogger<QueryGetAllCompanyHandler> _logger;

        public QueryGetAllCompanyHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, ILogger<QueryGetAllCompanyHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _logger = logger;
        }
        public async Task<PaginatedResponse<CompanyDTO>> Handle(QueryGetAllCompany request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Company.GetQueryable(
                    filter: f => f.IsActive && !f.IsDelete,
                    include: i => i.Include(s => s.Branches),
                    asNoTracking: true
                );

                // Apply pagination using extension method
                var result = await query.ToPaginatedResponseAsync(
                    request.PaginationRequest,
                    (company) =>
                    {
                        var companyDTO = _mapper.Map<Company, CompanyDTO>(company);

                        // Use extension method to encode all ID fields
                        companyDTO.EncodeIds(_hashId, company);

                        var listBranchDTO = new List<BranchDTO>();
                        foreach (var branch in company.Branches)
                        {
                            var branchDTO = _mapper.Map<Branch, BranchDTO>(branch);

                            // Use extension method to encode all ID fields
                            branchDTO.EncodeIds(_hashId, branch);

                            listBranchDTO.Add(branchDTO);
                        }

                        companyDTO.BranchDTOs = listBranchDTO;
                        return companyDTO;
                    }
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated companies");
                return new PaginatedResponse<CompanyDTO>(
                    new List<CompanyDTO>(),
                    0,
                    request.PaginationRequest.PageNumber,
                    request.PaginationRequest.PageSize
                );
            }
        }
    }
}
