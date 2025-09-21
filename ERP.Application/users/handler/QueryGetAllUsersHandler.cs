using AutoMapper;
using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
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
        public QueryGetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> Handle(QueryGetAllUsers request, CancellationToken cancellationToken)
        {
            var listUser = new List<UserDTO>();
            try
            {
                var users = await _unitOfWork.Users.GetManyAsync(f => f.IsActive && !f.IsDelete);
                listUser = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return listUser;
        }
    }
}
