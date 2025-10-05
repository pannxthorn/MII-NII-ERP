using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ApplicationDTO.users
{
    public class CreateUserDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Comment { get; set; }
    }
}
