using AutoMapper;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.Domain.entities;
using ERP.Shared._base.BaseMessage;
using ERP.Shared._base.BaseResponse;
using ERP.Shared.encryptservice;
using ERP.Shared.models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company.handler
{
    public class CommandCreateCompanyHandler : IRequestHandler<CommandCreateCompany, BaseResponse<CompanyDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;

        public CommandCreateCompanyHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
        }

        public async Task<BaseResponse<CompanyDTO>> Handle(CommandCreateCompany request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<CompanyDTO>();
            try
            {
                if (request.CurrentUser == null)
                    throw new UnauthorizedAccessException("User is not authenticated");

                return await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var company = await CreateCompany(request.Args, request.CurrentUser);
                    if (company != null)
                    {
                        var companyMapper = _mapper.Map<Company, CompanyDTO>(company);
                        companyMapper.CompanyId = _hashId.EncodeId(company.CompanyId);

                        companyMapper.Created_By_Id = _hashId.EncodeId(company.Created_By_Id);
                        companyMapper.Last_Update_By_Id = _hashId.EncodeId(company.Last_Update_By_Id);

                        var listBranch = new List<BranchDTO>();
                        foreach (var branch in company.Branches)
                        {
                            var branchMapper = _mapper.Map<Branch, BranchDTO>(branch);
                            branchMapper.BranchId = _hashId.EncodeId(branch.BranchId);
                            branchMapper.CompanyId = _hashId.EncodeId(branch.CompanyId);

                            branchMapper.Created_By_Id = _hashId.EncodeId(branch.Created_By_Id);
                            branchMapper.Last_Update_By_Id = _hashId.EncodeId(branch.Last_Update_By_Id);

                            listBranch.Add(branchMapper);
                        }
                        companyMapper.BranchDTOs = listBranch;

                        result.Data = companyMapper;
                        result.Message = BaseMessage.dataSuccess;
                        result.IsSuccess = true;
                    }
                    return result;
                });
            }
            catch (Exception ex)
            {
                return new BaseResponse<CompanyDTO>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Company?> CreateCompany(CreateCompanyDTO args, CurrentUserVM currentUser)
        {
            DateTime now = DateTime.UtcNow;
            if (!await _unitOfWork.Company.AnyAsync(f => f.CompanyCode == args.CompanyCode))
            {
                if (args.BranchDTOs != null)
                {
                    var companyMapper = _mapper.Map<CreateCompanyDTO, Company>(args);
                    companyMapper.IsActive = true;
                    companyMapper.IsDelete = false;

                    companyMapper.Created_By_Id = currentUser.UserId;
                    companyMapper.Creation_Date = now;

                    companyMapper.Last_Update_By_Id = currentUser.UserId;
                    companyMapper.Last_Update_By_Date = now;

                    foreach(var branch in args.BranchDTOs)
                    {
                        var branchMapper = _mapper.Map<CreateBranchDTO, Branch>(branch);
                        branchMapper.IsActive = true;
                        branchMapper.IsDelete = false;

                        branchMapper.Created_By_Id = currentUser.UserId;
                        branchMapper.Creation_Date = now;

                        branchMapper.Last_Update_By_Id = currentUser.UserId;
                        branchMapper.Last_Update_By_Date = now;

                        companyMapper.Branches.Add(branchMapper);
                        await _unitOfWork.Branch.AddAsync(branchMapper);
                    }

                    await _unitOfWork.Company.AddAsync(companyMapper);
                    await _unitOfWork.SaveChangesAsync();

                    return companyMapper;
                }
                else
                {
                    throw new Exception(string.Format(BaseMessage.requireBranchMsg, args.CompanyCode));
                }

            }
            else
            {
                throw new Exception(string.Format(BaseMessage.existsMsg, args.CompanyCode));
            }
        }
    }
}
