using ERP.Application._base;
using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class CommandCreateUser : IRequest<BaseResponse<UserDTO>>, ICurrentUserRequest
    {
        public CreateUserDTO Args { get; set; }
        public CurrentUserVM? CurrentUser { get; set; }

        public CommandCreateUser(CreateUserDTO args)
        {
            Args = args;
        }
    }
}
