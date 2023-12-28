using EFDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EFDataAccess;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog_Tag>()
            .HasOne(bt => bt.Blog)
            .WithMany(b => b.Blog_Tags)
            .HasForeignKey(bt => bt.BlogId);

        modelBuilder.Entity<Blog_Tag>()
            .HasOne(bt => bt.Tag)
            .WithMany(b => b.Blog_Tags)
            .HasForeignKey(bt => bt.TagId);

        modelBuilder.Entity<BlogType>()
            .HasData(
                new BlogType { Id = 1, Name = "архив" },
                new BlogType { Id = 2, Name = "черновик" }
            );
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BlogType> BlogTypes { get; set; }
    public DbSet<Blog_Tag> Blog_Tags { get; set; }
}
