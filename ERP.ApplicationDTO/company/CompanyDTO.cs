using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ApplicationDTO.branch;

namespace ERP.ApplicationDTO.company
{
    public class CompanyDTO
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty; // CompanyCode (length: 50)
        public string CompanyName { get; set; } = string.Empty; // CompanyName (length: 300)
        public string? TaxNo { get; set; } // TaxNo (length: 13)
        public string? Phone { get; set; } // Phone (length: 100)
        public string? Fax { get; set; } // Fax (length: 100)
        public string? Email { get; set; } // Email (length: 100)
        public string? Line { get; set; } // Line (length: 100)
        public string? Facebook { get; set; } // Facebook (length: 100)
        public string? Website { get; set; } // Website (length: 300)
        public string? Logo { get; set; } // Logo (length: 300)
        public string? Comment { get; set; } // Comment (length: 4000)
        public string Created_By_Id { get; set; } // Created_By_Id
        public System.DateTime Creation_Date { get; set; } // Creation_Date
        public string Last_Update_By_Id { get; set; } // Last_Update_By_Id
        public System.DateTime Last_Update_By_Date { get; set; } // Last_Update_By_Date
        public bool IsActive { get; set; } // IsActive
        public bool IsDelete { get; set; } // IsDelete

        public IEnumerable<BranchDTO> BranchDTOs { get; set; }
        public CompanyDTO()
        {
            BranchDTOs = new List<BranchDTO>();
        }
    }
}
