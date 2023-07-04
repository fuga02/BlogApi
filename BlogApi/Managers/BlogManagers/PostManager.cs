using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers.BlogManagers;

public class PostManager
{
    private readonly BlogDbContext _dbContext;

    public PostManager(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PostModel>> GetPosts()
    {
        var posts = await _dbContext.Posts.Include(p => p.Likes).Include(p => p.SavedPosts).ToListAsync();
        return ParseList(posts);
    }

    public async Task<List<PostModel>> GetPosts(Guid blogId)
    {
        var posts = await _dbContext.Posts.Where(p => p.BlogId ==  blogId).ToListAsync();
        return ParseList(posts);
    }

    public async Task<PostModel> GetPostById(Guid postId)
    {
        var post = IsExist(postId);
        return ParsePost(post);
    }

    public async Task<PostModel> CreatePost(CreatePostModel model)
    {
        var post = new Post()
        {
            Title = model.Title,
            Description = model.Description,
            BlogId = model.BlogId
        };
        _dbContext.Posts.Add(post);
        await _dbContext.SaveChangesAsync();
        return ParsePost(post);
    }

    public async Task<PostModel> UpdatePost(Guid postId, CreatePostModel model)
    {
        var post = IsExist(postId);
        post.Title = model.Title;
        post.Description = model.Description;
        post.UpdatedDate = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return ParsePost(post);
    }

    public async Task<string> DeletePost(Guid postId)
    {
        var post = IsExist(postId);
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
        return "Done :)";
    }

    public async Task<LikeModel> GetLikes(Guid postId, Guid userId)
    {
        var likes = await _dbContext.Likes.Where(l => l.PostId == postId).ToListAsync();
        var like = likes.FirstOrDefault(l => l.UserId == userId);
        return  new LikeModel()
        {
            IsLiked = like != null,
            Count = likes.Count
        };
    }

    public async Task<Like?> Like(Guid postId, Guid userId)
    {
        var like = await  _dbContext.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
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

    public async Task<bool> IsSaved(Guid postId, Guid userId)
    {
        return await _dbContext.SavedPosts
            .FirstOrDefaultAsync(s => s.PostId == postId && s.UserId == userId) != null;
    }

    public async Task<List<SavedPost>> GetSavedPosts(Guid userId)
    {
        var savedPosts =await  _dbContext.SavedPosts.Where(s => s.UserId == userId).ToListAsync();
        return savedPosts;
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


    private PostModel ParsePost(Post model)
    {
        var postModel = new PostModel()
        {
            PostId = model.PostId,
            Title = model.Title,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            Likes = model.Likes,
            SavedPosts = model.SavedPosts
        };

        return postModel;
    }
    private List<PostModel> ParseList(List<Post> posts)
    {
        var postModels = new List<PostModel>();
        foreach (var post in posts)
        {
            postModels.Add(ParsePost(post));
        }
        return postModels;
    }

    private Post IsExist(Guid postId)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.PostId == postId);
        if (post == null) throw new Exception("Not found");
        return post;
    }
}