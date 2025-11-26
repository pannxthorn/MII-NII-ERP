using ERP.ApplicationDTO.company;
using ERP.Shared._base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company.queries
{
    public class QueryGetAllCompany: IRequest<PaginatedResponse<CompanyDTO>>
    {
        public PaginationRequest PaginationRequest { get; set; }

        public QueryGetAllCompany(PaginationRequest paginationRequest)
        {
            PaginationRequest = paginationRequest;
        }
    }
}
