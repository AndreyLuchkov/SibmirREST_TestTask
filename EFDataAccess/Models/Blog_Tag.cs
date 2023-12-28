using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccess.Models;

[Table("Blog_Tag")]
public class Blog_Tag
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
