using AutoMapper;
using ERP.ApplicationDTO.branch;
using ERP.ApplicationDTO.company;
using ERP.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.company.profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDTO>();
            CreateMap<Branch, BranchDTO>();

            CreateMap<CreateCompanyDTO, Company>()
                .ForMember(c => c.Branches, m => m.Ignore())
                .ForMember(c => c.CompanyId, m => m.Ignore())
                .ForMember(c => c.Created_By_Id, m => m.Ignore())
                .ForMember(c => c.Last_Update_By_Id, m => m.Ignore());

            CreateMap<CreateBranchDTO, Branch>()
                .ForMember(b => b.BranchId, m => m.Ignore())
                .ForMember(b => b.CompanyId, m => m.Ignore())
                .ForMember(b => b.Created_By_Id, m => m.Ignore())
                .ForMember(b => b.Last_Update_By_Id, m => m.Ignore());

        }
    }
}
