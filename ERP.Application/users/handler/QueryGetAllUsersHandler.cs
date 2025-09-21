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
    public class QueryGetAllUsersHandler : IRequestHandler<QueryGetAllUsers, List<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashIdService _hashId;
        public QueryGetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper, IHashIdService hashId)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashId = hashId;
        }
        public async Task<List<UserDTO>> Handle(QueryGetAllUsers request, CancellationToken cancellationToken)
        {
            var listUser = new List<UserDTO>();
            try
            {
                var users = await _unitOfWork.Users.GetManyAsync(f => f.IsActive && !f.IsDelete);
                foreach (var u in users)
                {
                    var userDTO = _mapper.Map<User, UserDTO>(u);
                    userDTO.UserId = _hashId.EncodeId(u.UserId);
                    userDTO.CompanyId = u.CompanyId != null ? _hashId.EncodeId(u.CompanyId.GetValueOrDefault()) : null;
                    userDTO.BranchId = u.CompanyId != null ? _hashId.EncodeId(u.CompanyId.GetValueOrDefault()) : null;

                    listUser.Add(userDTO);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return listUser;
        }
    }
}
