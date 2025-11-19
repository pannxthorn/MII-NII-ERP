using AutoMapper;
using ERP.Application.Extensions;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.Application.users
{
    public class QueryGetUserByIdHandler : IRequestHandler<QueryGetUserById, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        private readonly ILogger<QueryGetUserByIdHandler> _logger;

        public QueryGetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId, ILogger<QueryGetUserByIdHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
            _logger = logger;
        }
        public async Task<UserDTO> Handle(QueryGetUserById request, CancellationToken cancellationToken)
        {
            var user = new UserDTO();
            try
            {
                var userModel = await _unitOfWork.Users.GetAsync(_hashId.DecodeToInt(request.Id));
                user = _mapper.Map<User, UserDTO>(userModel);

                // Use extension method to encode all ID fields
                user.EncodeIds(_hashId, userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", request.Id);
            }
            return user;
        }
    }
}
