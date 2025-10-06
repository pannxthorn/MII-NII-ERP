using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.branch
{
    public class BranchDTO
    {
        public string BranchId { get; set; } = string.Empty;// BranchId (Primary key)
        public string CompanyId { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty; // BranchCode (length: 50)
        public string BranchName { get; set; } = string.Empty; // BranchName (length: 300)
        public string? Phone { get; set; } // Phone (length: 100)
        public string? Email { get; set; } // Email (length: 100)
        public string? Line { get; set; } // Line (length: 100)
        public string? Facebook { get; set; } // Facebook (length: 100)
        public string? Comment { get; set; } // Comment (length: 4000)
        public bool IsHeadQuarter { get; set; } // IsHeadQuarter
        public string Created_By_Id { get; set; } // Created_By_Id
        public System.DateTime Creation_Date { get; set; } // Creation_Date
        public string Last_Update_By_Id { get; set; } // Last_Update_By_Id
        public System.DateTime Last_Update_By_Date { get; set; } // Last_Update_By_Date
        public bool IsActive { get; set; } // IsActive
        public bool IsDelete { get; set; } // IsDelete
    }
}
