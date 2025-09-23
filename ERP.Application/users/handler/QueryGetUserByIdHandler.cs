using AutoMapper;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using ERP.Shared.encryptservice;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class QueryGetUserByIdHandler : IRequestHandler<QueryGetUserById, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        public QueryGetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
        }
        public async Task<UserDTO> Handle(QueryGetUserById request, CancellationToken cancellationToken)
        {
            var user = new UserDTO();
            try
            {
                var userModel = await _unitOfWork.Users.GetAsync(_hashId.DecodeToInt(request.Id));
                user = _mapper.Map<User, UserDTO>(userModel);
                user.UserId = _hashId.EncodeId(userModel.UserId);
                user.CompanyId = userModel.CompanyId != null ? _hashId.EncodeId(userModel.CompanyId.GetValueOrDefault()) : null;
                user.BranchId = userModel.BranchId != null ? _hashId.EncodeId(userModel.BranchId.GetValueOrDefault()) : null;
                user.Created_By_Id = _hashId.EncodeId(userModel.Created_By_Id);
                user.Last_Update_By_Id = _hashId.EncodeId(userModel.Last_Update_By_Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return user;
        }
    }
}
