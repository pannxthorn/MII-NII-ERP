using AutoMapper;
using ERP.Application.company.events;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.branch;
using ERP.Domain.entities;
using ERP.EventBus.Interfaces;
using ERP.Shared._base.BaseMessage;
using ERP.Shared._base.BaseResponse;
using ERP.Shared.encryptservice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.branch.handler
{
    public class CommandCreateBranchHandler : IRequestHandler<CommandCreateBranch, BaseResponse<IEnumerable<BranchDTO>>>, IMessageHandler<CompanyCreatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly ILogger<CommandCreateBranchHandler> _logger;

        public CommandCreateBranchHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, ILogger<CommandCreateBranchHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _logger = logger;
        }
        public async Task<BaseResponse<IEnumerable<BranchDTO>>> Handle(CommandCreateBranch request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<IEnumerable<BranchDTO>>();
            try
            {
                return await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var res = await CreateBranch(request.Args);
                    if (res != null && res.Any())
                    {
                        var listBranch = new List<BranchDTO>();
                        foreach (var branch in res)
                        {
                            var branchMapper = _mapper.Map<Branch, BranchDTO>(branch);
                            branchMapper.BranchId = _hashId.EncodeId(branch.BranchId);
                            branchMapper.CompanyId = _hashId.EncodeId(branch.CompanyId);

                            branchMapper.Created_By_Id = _hashId.EncodeId(branch.Created_By_Id);
                            branchMapper.Last_Update_By_Id = _hashId.EncodeId(branch.Last_Update_By_Id);

                            listBranch.Add(branchMapper);
                        }

                        result.Data = listBranch;
                        result.Message = BaseMessage.dataSuccess;
                        result.IsSuccess = true;
                    }
                    return result;
                });
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<BranchDTO>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<IEnumerable<Branch>> CreateBranch(IEnumerable<CreateBranchDTO> args)
        {
            DateTime now = DateTime.UtcNow;
            var existsBranch = await _unitOfWork.Branch.GetManyAsync(f => args.Any(a => a.BranchCode == f.BranchCode) && f.IsActive == true && f.IsDelete == false);
            if (existsBranch == null)
            {
                var listBranch = new List<Branch>();
                foreach (var branch in args)
                {
                    var branchMapper = _mapper.Map<CreateBranchDTO, Branch>(branch);
                    branchMapper.IsActive = true;
                    branchMapper.IsDelete = false;

                    branchMapper.Created_By_Id = 1;
                    branchMapper.Creation_Date = now;

                    branchMapper.Last_Update_By_Id = 1;
                    branchMapper.Last_Update_By_Date = now;

                    await _unitOfWork.Branch.AddAsync(branchMapper);
                    listBranch.Add(branchMapper);
                }

                await _unitOfWork.SaveChangesAsync();
                return listBranch;
            }
            else
            {
                throw new Exception(string.Format(BaseMessage.existsMsg, string.Join(",", existsBranch.Select(f => f.BranchCode))));
            }
        }

        // Event Handler สำหรับ CompanyCreatedEvent
        public async Task HandleAsync(CompanyCreatedEvent message)
        {
            try
            {
                _logger.LogInformation("Processing CompanyCreatedEvent for CompanyId: {CompanyId}", message.CompanyId);

                if (message.BranchDTOs?.Any() == true)
                {
                    await _unitOfWork.ExecuteInTransactionAsync(async () =>
                    {
                        var listBranch = new List<Branch>();
                        DateTime now = DateTime.UtcNow;

                        foreach (var branchDto in message.BranchDTOs)
                        {
                            var branchMapper = _mapper.Map<CreateBranchDTO, Branch>(branchDto);
                            branchMapper.CompanyId = message.CompanyId; // Set CompanyId from event
                            branchMapper.IsActive = true;
                            branchMapper.IsDelete = false;

                            branchMapper.Created_By_Id = 1;
                            branchMapper.Creation_Date = now;

                            branchMapper.Last_Update_By_Id = 1;
                            branchMapper.Last_Update_By_Date = now;

                            await _unitOfWork.Branch.AddAsync(branchMapper);
                            listBranch.Add(branchMapper);
                        }

                        await _unitOfWork.SaveChangesAsync();

                        _logger.LogInformation("Successfully created {BranchCount} branches for Company: {CompanyCode}",
                            listBranch.Count, message.CompanyCode);

                        return listBranch;
                    });
                }
                else
                {
                    _logger.LogWarning("No branches to create for Company: {CompanyCode}", message.CompanyCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process CompanyCreatedEvent for CompanyId: {CompanyId}", message.CompanyId);
                throw;
            }
        }
    }
}
