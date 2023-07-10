using BlogApi.Context;
using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public interface IBlogRepository
{
     Task<List<Blog>> GetBlogs();

     Task<Blog?> GetBlogById(Guid id);

     Task<List<Blog>> GetBlogByAuthor(Guid userId);
     Task CreateBlog(Blog blog);
     Task UpdateBlog(Blog blog);
        
     Task DeleteBlog(Blog blog);
}

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _dbContext;

    public BlogRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Blog>> GetBlogs()
    {

        return await _dbContext.Blogs.ToListAsync();
    }

    public async Task<Blog?> GetBlogById(Guid id)
    {
        return await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Blog>> GetBlogByAuthor(Guid userId)
    {
        return await _dbContext.Blogs.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task CreateBlog(Blog blog)
    {
        _dbContext.Blogs.Add(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBlog(Blog blog)
    {
        _dbContext.Blogs.Update(blog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteBlog(Blog blog)
    {
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync();
    }
}