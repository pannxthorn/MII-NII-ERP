using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using MediatR;

namespace ERP.Application.users
{
    public class CommandLogin : IRequest<BaseResponse<LoginResponseDTO>>
    {
        public LoginDTO Args { get; set; }
        public CommandLogin(LoginDTO args)
        {
            Args = args;
        }
    }
}
