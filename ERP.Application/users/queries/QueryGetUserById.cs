using ERP.ApplicationDTO.users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class QueryGetUserById : IRequest<UserDTO>
    {
        public string Id { get; set; }
        public QueryGetUserById(string userId)
        {
            Id = userId;
        }
    }
}
