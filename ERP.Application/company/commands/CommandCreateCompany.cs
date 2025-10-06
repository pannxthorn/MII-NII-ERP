using ERP.ApplicationDTO.company;
using ERP.Shared._base.BaseResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company
{
    public class CommandCreateCompany : IRequest<BaseResponse<CompanyDTO>>
    {
        public CreateCompanyDTO Args { get; set; }
        public CommandCreateCompany(CreateCompanyDTO args)
        {
            Args = args;
        }
    }
}
