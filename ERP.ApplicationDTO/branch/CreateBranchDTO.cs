using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.branch
{
    public class CreateBranchDTO
    {
        public string BranchCode { get; set; } = string.Empty; // BranchCode (length: 50)
        public string BranchName { get; set; } = string.Empty; // BranchName (length: 300)
        public string? Phone { get; set; } // Phone (length: 100)
        public string? Email { get; set; } // Email (length: 100)
        public string? Line { get; set; } // Line (length: 100)
        public string? Facebook { get; set; } // Facebook (length: 100)
        public string? Comment { get; set; } // Comment (length: 4000)
        public bool IsHeadQuarter { get; set; } // IsHeadQuarter
    }
}
