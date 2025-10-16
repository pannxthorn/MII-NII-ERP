using System.ComponentModel.DataAnnotations;

namespace ERP.ApplicationDTO.users
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
