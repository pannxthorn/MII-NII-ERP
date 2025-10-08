using AutoMapper;
using ERP.Application.company.queries;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.company;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.ApplicationDTO.branch;

namespace ERP.Application.company.handler
{
    public class QueryGetAllCompanyHandler : IRequestHandler<QueryGetAllCompany, List<CompanyDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        public QueryGetAllCompanyHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
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
                    companyDTO.CompanyId = _hashId.EncodeId(company.CompanyId);
                    companyDTO.Created_By_Id = _hashId.EncodeId(company.Created_By_Id);
                    companyDTO.Last_Update_By_Id = _hashId.EncodeId(company.Last_Update_By_Id);

                    var listBranchDTO = new List<BranchDTO>();
                    foreach (var branch in company.Branches)
                    {
                        var branchDTO = _mapper.Map<Branch, BranchDTO>(branch);
                        branchDTO.BranchId = _hashId.EncodeId(branch.BranchId);
                        branchDTO.CompanyId = _hashId.EncodeId(branch.CompanyId);
                        branchDTO.Created_By_Id = _hashId.EncodeId(branch.Created_By_Id);
                        branchDTO.Last_Update_By_Id = _hashId.EncodeId(branch.Last_Update_By_Id);

                        listBranchDTO.Add(branchDTO);
                    }

                    companyDTO.BranchDTOs = listBranchDTO;
                    listCompany.Add(companyDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return listCompany;
        }
    }
}
