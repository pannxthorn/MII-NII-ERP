using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.users
{
    public class CreateUserDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string CompanyId { get; set; }
        public required string BranchId { get; set; }
        public string? Comment { get; set; }
    }
}
