using AutoMapper;
using ERP.Application.Extensions;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP.Application.users
{
    public class QueryGetAllUsersHandler : IRequestHandler<QueryGetAllUsers, List<UserDTO>>
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
        public async Task<List<UserDTO>> Handle(QueryGetAllUsers request, CancellationToken cancellationToken)
        {
            var listUser = new List<UserDTO>();
            try
            {
                var users = await _unitOfWork.Users.GetManyAsync(f => f.IsActive && !f.IsDelete,
                                                                      includes: i => i.Include(c => c.Company)
                                                                                        .ThenInclude(b => b.Branches));
                foreach (var u in users)
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

                    listUser.Add(userDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
            }

            return listUser;
        }
    }
}
