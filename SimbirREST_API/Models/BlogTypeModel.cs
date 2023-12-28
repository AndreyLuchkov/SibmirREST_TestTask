using EFDataAccess;
using System.ComponentModel.DataAnnotations;

namespace SimbirREST_API.Models
{
    public class BlogTypeModel
    {
        [Required]
        public string Name { get; set; }
    }
}
