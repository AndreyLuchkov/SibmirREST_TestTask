using System.ComponentModel.DataAnnotations;

namespace SimbirREST_API.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime? PublicationDate { get; set; }

        [Required]
        public AuthorModel Author { get; set; }
        [Required]
        public BlogTypeModel BlogType { get; set; }
        public List<TagModel> Tags { get; set; } = [];
    }
}
