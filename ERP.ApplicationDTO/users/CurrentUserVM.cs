using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.users
{
    public class CurrentUserVM
    {
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
