using System.ComponentModel.DataAnnotations;

namespace SimbirREST_API.Models
{
    public class AuthorModel
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
