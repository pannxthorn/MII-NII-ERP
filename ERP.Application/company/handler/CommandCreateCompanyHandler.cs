using AutoMapper;
using ERP.Application.company.events;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared._base.BaseMessage;
using ERP.Shared._base.BaseResponse;
using ERP.Shared.encryptservice;
using ERP.EventBus;
using ERP.EventBus.Interfaces;
using MediatR;

namespace ERP.Application.company.handler
{
    public class CommandCreateCompanyHandler : IRequestHandler<CommandCreateCompany, BaseResponse<CompanyDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly IEventBus _eventBus;

        public CommandCreateCompanyHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, IEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _eventBus = eventBus;
        }

        public async Task<BaseResponse<CompanyDTO>> Handle(CommandCreateCompany request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<CompanyDTO>();
            try
            {
                return await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var company = await CreateCompany(request.Args);
                    if (company != null)
                    {
                        var companyMapper = _mapper.Map<Company, CompanyDTO>(company);
                        companyMapper.CompanyId = _hashId.EncodeId(company.CompanyId);

                        companyMapper.Created_By_Id = _hashId.EncodeId(company.Created_By_Id);
                        companyMapper.Last_Update_By_Id = _hashId.EncodeId(company.Last_Update_By_Id);

                        // Publish event for branch creation
                        await _eventBus.PublishAsync(new CompanyCreatedEvent
                        {
                            CompanyId = company.CompanyId,
                            CompanyCode = company.CompanyCode,
                            CompanyName = company.CompanyName,
                            BranchDTOs = request.Args.BranchDTOs?.ToList() ?? new List<CreateBranchDTO>()
                        });

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

        public async Task<Company?> CreateCompany(CreateCompanyDTO args)
        {
            DateTime now = DateTime.UtcNow;
            if (!await _unitOfWork.Company.AnyAsync(f => f.CompanyCode == args.CompanyCode))
            {
                if (args.BranchDTOs != null)
                {
                    var companyMapper = _mapper.Map<CreateCompanyDTO, Company>(args);
                    companyMapper.IsActive = true;
                    companyMapper.IsDelete = false;

                    companyMapper.Created_By_Id = 1;
                    companyMapper.Creation_Date = now;

                    companyMapper.Last_Update_By_Id = 1;
                    companyMapper.Last_Update_By_Date = now;

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
