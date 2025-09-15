using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.users
{
    public class UserDTO
    {
        public int UserId { get; set; } // UserId (Primary key)
        public int? CompanyId { get; set; } // CompanyId
        public int? BranchId { get; set; } // BranchId
        public string? UserName { get; set; } // UserName (length: 100)
        public string? Password { get; set; } // Password (length: 500)
        public string? Comment { get; set; } // Comment (length: 4000)
        public bool IsActive { get; set; } // IsActive
        public bool IsDelete { get; set; } // IsDelete
    }
}
