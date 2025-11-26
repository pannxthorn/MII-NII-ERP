using ERP.ApplicationDTO.users;
using ERP.Shared._base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class QueryGetAllUsers : IRequest<PaginatedResponse<UserDTO>>
    {
        public PaginationRequest PaginationRequest { get; set; }

        public QueryGetAllUsers(PaginationRequest paginationRequest)
        {
            PaginationRequest = paginationRequest;
        }
    }
}
