using ERP.Application._base;
using ERP.ApplicationDTO.company;
using ERP.ApplicationDTO.users;
using ERP.Shared._base.BaseResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company
{
    public class CommandCreateCompany : IRequest<BaseResponse<CompanyDTO>>, ICurrentUserRequest
    {
        public CreateCompanyDTO Args { get; set; }
        public CurrentUserVM? CurrentUser { get; set; }

        public CommandCreateCompany(CreateCompanyDTO args)
        {
            Args = args;
        }
    }
}
