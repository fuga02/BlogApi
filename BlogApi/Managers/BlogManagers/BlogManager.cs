using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using BlogApi.Providers;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers.BlogManagers;

public class BlogManager
{
    private readonly BlogDbContext _dbContext;
    private readonly UserProvider _userProvider;
    private readonly PostManager _postManager;

    public BlogManager(BlogDbContext dbContext, UserProvider userProvider, PostManager postManager)
    {
        _dbContext = dbContext;
        _userProvider = userProvider;
        _postManager = postManager;
    }

    public async Task<List<BlogModel>> GetBlogs()
    {
        var blogs = await _dbContext.Blogs.ToListAsync();

        return ParseList(blogs);
    }

    public async Task<BlogModel> GetBlogById(Guid id)
    {
        var blog = IsExist(id);

        return ParseToBlogModel(blog);
    }

    public async Task<List<BlogModel>> GetBlogByAuthor()
    {
        var userId = _userProvider.UserId;
        var blogs =  await _dbContext.Blogs.Where(b => b.UserId == userId).ToListAsync();
        if (blogs == null) throw new Exception("Not found");

        return  ParseList(blogs);
    }
    public async Task<List<BlogModel>> GetBlogByAuthor(string username)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) throw new Exception("Not found such kinda user");
        var blogs = user.Blogs;
        if (blogs == null) throw new Exception("This user has no blogs");

        return ParseList(blogs);
    }
    public async Task<BlogModel> CreateBlog(CreateBlogModel model)
    {
        var blog = new Blog()
        {
            Name = model.Name,
            Description = model.Description,
            UserId = _userProvider.UserId
        };
        _dbContext.Blogs.Add(blog);
        await _dbContext.SaveChangesAsync();
        return ParseToBlogModel(blog);
    }
    public async Task<BlogModel> UpdateBlog(Guid blogId,CreateBlogModel model)
    {
        var blog =  IsExist(blogId);
        blog.Name = model.Name;
        blog.Description = model.Description;
        await _dbContext.SaveChangesAsync();
        return ParseToBlogModel(blog);
    }

    public async Task<string> DeleteBlog(Guid blogId)
    {
        var blog = IsExist(blogId);
        _dbContext.Blogs.Remove(blog);
        await _dbContext.SaveChangesAsync();
        return "Done :)";
    }

    private BlogModel ParseToBlogModel(Blog blog)
    {
        var blogModel = new BlogModel()
        {
            Id = blog.Id,
            Name = blog.Name,
            Description = blog.Description!,
            CreatedDate = blog.CreatedDate,
            UserId = blog.UserId,
            UserName = _userProvider.UserName,
            Posts = _postManager.ParseListPostModel(blog.Posts),
        };
        return  blogModel;
    }
    

    private List<BlogModel> ParseList(List<Blog> blogs)
    {
        var blogModels = new List<BlogModel>();
        foreach (var blog in blogs)
        {
            blogModels.Add(ParseToBlogModel(blog));
        }
        return blogModels;
    }

    private Blog IsExist(Guid blogId)
    {
        var blog =  _dbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
        if (blog == null) throw new Exception("Not found");
        return blog;
    } 
}