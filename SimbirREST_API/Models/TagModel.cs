using System.ComponentModel.DataAnnotations;

namespace SimbirREST_API.Models
{
    public class TagModel
    {
        [Required]
        public string Name { get; set; }
    }
}
