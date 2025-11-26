using AutoMapper;
using ERP.Application.Extensions;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared._base;
using ERP.Shared.encryptservice;
using ERP.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP.Application.users
{
    public class QueryGetAllUsersHandler : IRequestHandler<QueryGetAllUsers, PaginatedResponse<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly ILogger<QueryGetAllUsersHandler> _logger;

        public QueryGetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, ILogger<QueryGetAllUsersHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _logger = logger;
        }
        public async Task<PaginatedResponse<UserDTO>> Handle(QueryGetAllUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Users.GetQueryable(
                    filter: f => f.IsActive && !f.IsDelete,
                    include: i => i.Include(c => c.Company).ThenInclude(b => b.Branches),
                    asNoTracking: true
                );

                // Apply pagination using extension method
                var result = await query.ToPaginatedResponseAsync(
                    request.PaginationRequest,
                    (u) =>
                    {
                        var userDTO = _mapper.Map<User, UserDTO>(u);

                        // Use extension method to encode all ID fields
                        userDTO.EncodeIds(_hashId, u);

                        if (u.Company != null)
                        {
                            var branch = u.Company.Branches.FirstOrDefault(f => f.BranchId == u.BranchId);
                            if (branch != null)
                            {
                                userDTO.BranchName = branch.BranchName;
                            }

                            userDTO.CompanyName = u.Company.CompanyName;
                        }

                        return userDTO;
                    }
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated users");
                return new PaginatedResponse<UserDTO>(
                    new List<UserDTO>(),
                    0,
                    request.PaginationRequest.PageNumber,
                    request.PaginationRequest.PageSize
                );
            }
        }
    }
}
