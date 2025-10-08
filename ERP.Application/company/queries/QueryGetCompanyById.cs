using ERP.ApplicationDTO.company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company.queries
{
    public class QueryGetCompanyById : IRequest<CompanyDTO>
    {
        public string Id { get; set; }
        public QueryGetCompanyById(string companyId)
        {
            Id = companyId;
        }
    }
}
