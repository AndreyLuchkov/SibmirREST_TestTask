using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models;

[Table("Blog")]
public class Blog
{
    public int Id { get; set; }
    [Required]
    [MaxLength(300)]
    public string Name { get; set; } = null!;
    [MaxLength(1000)]
    public string? Description { get; set; }
    [MaxLength(200)]
    public string? ShortDescription { get; set; }
    [Required]
    public DateTime CreatedOn { get; set; }
    [Required]
    public DateTime ModifiedOn { get; set; }
    public DateTime? PublicationDate { get; set; }

    [Column("AuthorId")]
    public int AuthorId { get; set; }

    [Column("BlogTypeId")]
    public int BlogTypeId { get; set; }

    public Author Author { get; set; } = null!;
    public BlogType BlogType { get; set; } = null!;
    public List<Blog_Tag> Blog_Tags { get; set; } = [];
}
