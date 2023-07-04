using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Context;

public class BlogDbContext:DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Blogs)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);
        modelBuilder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
}