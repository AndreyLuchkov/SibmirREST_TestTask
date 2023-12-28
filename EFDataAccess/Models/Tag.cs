using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models;

[Table("Tag")]
public class Tag
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public List<Blog_Tag> Blog_Tags { get; set; } = [];
}
