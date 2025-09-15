using ERP.Application.unitofwork;
using ERP.ApplicationDTO.users;
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
        public QueryGetAllUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserDTO>> Handle(QueryGetAllUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.Users.GetManyAsync(f => f.IsActive && !f.IsDelete);

                return users.Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    CompanyId = u.CompanyId,
                    BranchId = u.BranchId,
                    UserName = u.UserName,
                    Password = u.Password,
                    Comment = u.Comment,
                    IsActive = u.IsActive,
                    IsDelete = u.IsDelete,
                }).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<UserDTO>();
            }
        }
    }
}
