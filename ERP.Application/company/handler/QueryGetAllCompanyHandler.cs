using AutoMapper;
using ERP.Application.company.queries;
using ERP.Application.Extensions;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP.Application.company.handler
{
    public class QueryGetAllCompanyHandler : IRequestHandler<QueryGetAllCompany, List<CompanyDTO>>
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
        public async Task<List<CompanyDTO>> Handle(QueryGetAllCompany request, CancellationToken cancellationToken)
        {
            var listCompany = new List<CompanyDTO>();
            try
            {
                var companys = await _unitOfWork.Company.GetManyAsync(f => f.IsActive && !f.IsDelete, i => i.Include(s => s.Branches),
                                                                            asNoTracking: true, useSplitQuery: true);
                foreach (var company in companys)
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
                    listCompany.Add(companyDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all companies");
            }

            return listCompany;
        }
    }
}
