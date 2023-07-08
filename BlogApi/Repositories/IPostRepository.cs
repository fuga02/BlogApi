using BlogApi.Context;
using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public interface IPostRepository
{
     Task<List<Post>> GetPosts();
    
     Task<Post?> GetPostById(Guid postId);

     Task CreatePost(Post post);
    
     Task UpdatePost(Post post);

     Task DeletePost(Post post);

     Task<List<Like>> GetLikes(Guid postId);

     Task<Like?> Like(Guid postId, Guid userId);
    
     Task<List<SavedPost>> GetSavedPosts(Guid userId);
    
     Task<SavedPost?> SavePost(Guid postId, Guid userId);
    
     Task<List<Comment>> GetComments();

     Task<List<Comment>> GetComments(Guid postId);

     Task CreateComment(Comment comment);

     Task UpdateComment(Comment comment);
    
     Task DeleteComment(Comment comment);
     Task<Comment?> GetCommentById(Guid commentId);



}

public class PostRepository: IPostRepository
{
    private readonly BlogDbContext _dbContext;

    public PostRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Post>> GetPosts()
    {
        return await _dbContext.Posts.Include(p => p.Likes).ToListAsync();
    }

    public async Task<Post?> GetPostById(Guid postId)
    {
        return await _dbContext.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
    }

    public async Task CreatePost(Post post)
    {
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePost(Post post)
    {
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePost(Post post)
    {
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Like>> GetLikes(Guid postId)
    {
        return _dbContext.Likes.Where(l => l.PostId == postId).ToList();
    }

    public async Task<Like?> Like(Guid postId, Guid userId)
    {

        var like = await _dbContext.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
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

    public async Task<List<SavedPost>> GetSavedPosts(Guid userId)
    {
        return  await _dbContext.SavedPosts.Where(s => s.UserId == userId).ToListAsync();
    }

    public async Task<SavedPost?> SavePost(Guid postId, Guid userId)
    {
        var savedPost = await _dbContext.SavedPosts.FirstOrDefaultAsync(s => s.PostId == postId && s.UserId == userId);
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

    public async Task<List<Comment>> GetComments()
    {
        return await _dbContext.Comments.ToListAsync();
    }

    public async Task<List<Comment>> GetComments(Guid postId)
    {
        return await _dbContext.Comments.Where(p => p.PostId == postId).ToListAsync();
    }

    public async Task CreateComment(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateComment(Comment comment)
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteComment(Comment comment)
    {
         _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Comment?> GetCommentById(Guid commentId)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
    }
}