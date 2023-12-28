using System.ComponentModel.DataAnnotations;

namespace SimbirREST_API.Models
{
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
