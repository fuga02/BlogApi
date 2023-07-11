using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using BlogApi.Providers;
using BlogApi.Repositories;

namespace BlogApi.Managers.BlogManagers;

public class BlogManager
{
    private readonly UserProvider _userProvider;
    private readonly PostManager _postManager;
    private readonly IBlogRepository _blogRepository;   

    public BlogManager( UserProvider userProvider, PostManager postManager, IBlogRepository blogRepository)
    {
        _userProvider = userProvider;
        _postManager = postManager;
        _blogRepository = blogRepository;
    }

    public async Task<List<BlogModel>> GetBlogs()
    {
        var blogs = await _blogRepository.GetBlogs();

        return await ParseList(blogs);
    }

    public async Task<BlogModel> GetBlogById(Guid id)
    {
        var blog = await IsExist(id);

        return await ParseToBlogModel(blog);
    }

    public async Task<List<BlogModel>> GetBlogByAuthor()
    {
        var userId = _userProvider.UserId;
        var blogs =  await _blogRepository.GetBlogByAuthor(userId);
        if (blogs == null) throw new Exception("Not found");

        return  await ParseList(blogs);
    }
    public async Task<BlogModel> CreateBlog(CreateBlogModel model)
    {
        var blog = new Blog()
        {
            Name = model.Name,
            Description = model.Description,
            UserId = _userProvider.UserId
        };
        await _blogRepository.CreateBlog(blog);
        return await ParseToBlogModel(blog);;
    }
    public async Task<BlogModel> UpdateBlog(Guid blogId,CreateBlogModel model)
    {
        var blog =  await IsExist(blogId);
        blog.Name = model.Name;
        blog.Description = model.Description;
        await _blogRepository.UpdateBlog(blog);
        return await ParseToBlogModel(blog);
    }

    public async Task<string> DeleteBlog(Guid blogId)
    {
        var blog = await IsExist(blogId);
        await _blogRepository.DeleteBlog(blog);
        return "Done :)";
    }

    private async Task<BlogModel> ParseToBlogModel(Blog blog)
    {
        var blogModel = new BlogModel()
        {
            Id = blog.Id,
            Name = blog.Name,
            Description = blog.Description!,
            CreatedDate = blog.CreatedDate,
            UserId = blog.UserId,
            UserName = _userProvider.UserName,
            Posts = await _postManager.ParseListPostModel(blog.Posts),
        };
        return  blogModel;
    }
    

    private async Task<List<BlogModel>> ParseList(List<Blog> blogs)
    {
        var blogModels = new List<BlogModel>();
        foreach (var blog in blogs)
        {
            blogModels.Add(await ParseToBlogModel(blog));
        }
        return blogModels;
    }

    public async Task<Blog> IsExist(Guid blogId)
    {
        var blog =  await _blogRepository.GetBlogById(blogId);
        if (blog == null) throw new Exception("Not found");
        return blog;
    } 
}