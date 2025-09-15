using ERP.ApplicationDTO.users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class QueryGetAllUsers : IRequest<List<UserDTO>>
    {
    }
}
