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
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<User> Users { get; set; }
}