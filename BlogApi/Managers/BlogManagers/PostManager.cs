using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using BlogApi.Providers;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers.BlogManagers;

public class PostManager
{
    private readonly BlogDbContext _dbContext;
    private readonly UserProvider _userProvider;

    public PostManager(BlogDbContext dbContext, UserProvider userProvider)
    {
        _dbContext = dbContext;
        _userProvider = userProvider;
    }

    public async Task<List<PostModel>> GetPosts()
    {
        var posts = await _dbContext.Posts.Include(p => p.Likes).Include(p => p.SavedPosts).ToListAsync();
        return ParseList(posts);
    }

    /*
    public async Task<List<PostModel>> GetPosts(Guid blogId)
    {
        var posts = await _dbContext.Posts.Where(p => p.BlogId ==  blogId).ToListAsync();
        return ParseList(posts);
    }*/

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


    public async Task<Like_Saved_Model?> Like(Guid postId)
    {
        var userId = _userProvider.UserId;
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
            return Parse_Like_Saved_Model(like);

        }
        else
        {
            _dbContext.Likes.Remove(like);
            await _dbContext.SaveChangesAsync();
            return null;
        }
    }


    public async Task<List<SavedPost>> GetSavedPosts()
    {
        var userId = _userProvider.UserId;
        var savedPosts =await  _dbContext.SavedPosts.Where(s => s.UserId == userId).ToListAsync();
        return savedPosts;
    }

    public async Task<Like_Saved_Model?> SavePost(Guid postId)
    {
        var userId = _userProvider.UserId;
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
            return Parse_Like_Saved_Model(savedPost);
        }
        else
        {
            _dbContext.SavedPosts.Remove(savedPost);
            await _dbContext.SaveChangesAsync();
            return null;
        }
    }

    private Like_Saved_Model Parse_Like_Saved_Model( SavedPost model )
    {
        var savedPostModel = new Like_Saved_Model()
        {
            Id = model.Id,
            PostId = model.PostId,
            UserId = model.UserId
        };
        return savedPostModel;
    }
    private Like_Saved_Model Parse_Like_Saved_Model( Like model )
    {
        var savedPostModel = new Like_Saved_Model()
        {
            Id = model.Id,
            PostId = model.PostId,
            UserId = model.UserId
        };
        return savedPostModel;
    }

    private  PostModel ParsePost(Post model)
    {
        Tuple<bool,int>like =  GetLikes(model.PostId);
        var postModel = new PostModel()
        {
            PostId = model.PostId,
            Title = model.Title,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            IsLiked = like.Item1,
            LikeCount = like.Item2,
            Likes = model.Likes,
            IsSaved = IsSaved(model.PostId),
            BlogId = model.BlogId,
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




    public bool IsSaved(Guid postId)
    {
        var userId = _userProvider.UserId;
        return _dbContext.SavedPosts
            .FirstOrDefault(s => s.PostId == postId && s.UserId == userId) != null;
    }


    public Tuple<bool, int> GetLikes(Guid postId)
    {
        var userId = _userProvider.UserId;
        var likes = _dbContext.Likes.Where(l => l.PostId == postId).ToList();
        var like = likes.FirstOrDefault(l => l.UserId == userId);
        return new Tuple<bool, int>(like != null, likes.Count);
    }
}