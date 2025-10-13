using ERP.ApplicationDTO.branch;
using ERP.Shared._base.BaseResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.branch
{
    public class CommandCreateBranch : IRequest<BaseResponse<IEnumerable<BranchDTO>>>
    {
        public IEnumerable<CreateBranchDTO> Args { get; set; }
        public CommandCreateBranch(IEnumerable<CreateBranchDTO> args)
        {
            Args = args;
        }
    }
}
