using AutoMapper;
using ERP.ApplicationDTO.users;
using ERP.Domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.users
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDTO, User>()
                .ForMember(u => u.UserId, m => m.Ignore())
                .ForMember(u => u.CompanyId, m => m.Ignore())
                .ForMember(u => u.BranchId, m => m.Ignore())
                .ForMember(u => u.Created_By_Id, m => m.Ignore())
                .ForMember(u => u.Last_Update_By_Id, m => m.Ignore());
        }
    }
}
