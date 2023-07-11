using BlogApi.Context;
using BlogApi.Entities;

namespace BlogApi.Repositories;

public interface IPostRepository
{
     Task<List<Post>> GetPosts(Guid blogId);
    
     Task<Post> GetPostById(Guid blogId,Guid postId);

     Task CreatePost(Guid blogId,Post post);
    
     Task UpdatePost(Guid blogId,Post post);

     Task DeletePost(Guid blogId, Post post);

     Task<List<Like>> GetLikes(Guid blogId,Guid postId);

     Task<Like?> Like(Guid blogId,Guid postId, Guid userId);
    
     Task<List<SavedPost>> GetSavedPosts(Guid blogId,Guid postId , Guid userId);
    
     Task<SavedPost?> SavePost(Guid blogId, Guid postId, Guid userId);
    
     Task<List<Comment>> GetComments(Guid blogId, Guid postId);
    

     Task CreateComment(Guid blogId,Guid postId, Comment comment);

     Task UpdateComment(Guid blogId,Guid postId, Comment comment);
    
     Task DeleteComment(Guid blogId,Guid postId, Comment comment);
     Task<Comment?> GetCommentById(Guid blogId,Guid postId, Guid commentId);



}

public class PostRepository: IPostRepository
{
    private readonly BlogDbContext _dbContext;
    private readonly IBlogRepository _blogRepository;

    public PostRepository(BlogDbContext dbContext, IBlogRepository blogRepository)
    {
        _dbContext = dbContext;
        _blogRepository = blogRepository;
    }

    public async Task<List<Post>> GetPosts(Guid blogId)
    {
        var blog = await GetBlogById(blogId);
        return blog.Posts;
    }

    public async Task<Post> GetPostById(Guid blogId, Guid postId)
    {
        var blog = await GetBlogById(blogId);
        return await GetPostByBlog(blog,postId);
    }

    public async Task CreatePost(Guid blogId, Post post)
    {
        var blog = await GetBlogById(blogId);
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePost(Guid blogId, Post post)
    {
        var blog = await GetBlogById(blogId);
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task DeletePost(Guid blogId, Post post)
    {
        var blog = await GetBlogById(blogId);
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Like>> GetLikes(Guid blogId,Guid postId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog,postId);
        return post.Likes;
    }

    public async Task<Like?> Like(Guid blogId,Guid postId, Guid userId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog,postId);
        var like = post.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);
        if (like == null)
        {
            like = new Like()
            {
                PostId = postId,
                UserId = userId
            };
            _dbContext.Likes.Add(like);
            await _dbContext.SaveChangesAsync();
            return like;

        }
        else
        {
            _dbContext.Likes.Remove(like);
            await _dbContext.SaveChangesAsync();
            return null;
        }

    }

    public async Task<List<SavedPost>> GetSavedPosts(Guid blogId,Guid postId, Guid userId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog,postId);
        return post.SavedPosts.Where(s => s.UserId == userId).ToList();
    }

    public async Task<SavedPost?> SavePost(Guid blogId, Guid postId, Guid userId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        var savedPost = post.SavedPosts.FirstOrDefault(s => s.PostId == postId && s.UserId == userId);
        if (savedPost == null)
        {
            savedPost = new SavedPost()
            {
                PostId = postId,
                UserId = userId
            };
            _dbContext.SavedPosts.Add(savedPost);
            await _dbContext.SaveChangesAsync();
            return savedPost;
        }
        else
        {
            _dbContext.SavedPosts.Remove(savedPost);
            await _dbContext.SaveChangesAsync();
            return null;
        }
    }
    

    public async Task<List<Comment>> GetComments(Guid blogId,Guid postId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        return  post.Comments.Where(p => p.PostId == postId).ToList();
    }

    public async Task CreateComment(Guid blogId,Guid postId, Comment comment)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateComment(Guid blogId, Guid postId,Comment comment)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid blogId, Guid postId,Comment comment)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Comment?> GetCommentById(Guid blogId, Guid postId,Guid commentId)
    {
        var blog = await GetBlogById(blogId);
        var post = await GetPostByBlog(blog, postId);
        return post.Comments.FirstOrDefault(c => c.Id == commentId);
    }

    private async Task<Blog> GetBlogById(Guid blogId)
    {
        var blog = await _blogRepository.GetBlogById(blogId);
        if (blog != null) return blog;
        throw new Exception("Blog Not found");
    }

    private async Task<Post> GetPostByBlog(Blog blog, Guid postId)
    {
        var post = blog.Posts.FirstOrDefault(p => p.PostId == postId);
        if (post != null) return post;
        throw new Exception("Post Not found");
    }
}