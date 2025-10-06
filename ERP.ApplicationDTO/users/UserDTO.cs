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
        public string UserId { get; set; } = string.Empty; // UserId (Primary key)
        public string? CompanyId { get; set; } // CompanyId
        public string? BranchId { get; set; } // BranchId
        public string? UserName { get; set; } // UserName (length: 100)
        public string? Comment { get; set; } // Comment (length: 4000)
        public bool IsActive { get; set; } // IsActive
        public bool IsDelete { get; set; } // IsDelete
        public string? Created_By_Id { get; set; }
        public DateTime? Creation_Date { get; set; }
        public string? Last_Update_By_Id { get; set; }
        public DateTime? Last_Update_By_Date { get; set; }
    }
}
