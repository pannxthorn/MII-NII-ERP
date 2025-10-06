using ERP.ApplicationDTO.branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.company
{
    public class CreateCompanyDTO
    {
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

        public IEnumerable<CreateBranchDTO> BranchDTOs { get; set; }
        public CreateCompanyDTO()
        {
            BranchDTOs = new List<CreateBranchDTO>();
        }
    }
}
