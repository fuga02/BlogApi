using BlogApi.Entities;
using BlogApi.Models.BlogModels;
using BlogApi.Providers;
using BlogApi.Repositories;

namespace BlogApi.Managers.BlogManagers;

public class PostManager
{
    private readonly UserProvider _userProvider;
    private readonly IPostRepository _postRepository;

    public PostManager(UserProvider userProvider, IPostRepository postRepository)
    {
        _userProvider = userProvider;
        _postRepository = postRepository;
    }

    public async Task<List<PostModel>> GetPosts()
    {
        var posts = await _postRepository.GetPosts();
        return await ParseListPostModel(posts);
    }

    /*
    public async Task<List<PostModel>> GetPosts(Guid blogId)
    {
        var posts = await _dbContext.Posts.Where(p => p.BlogId ==  blogId).ToListAsync();
        return ParseList(posts);
    }*/

    public async Task<PostModel> GetPostById(Guid postId)
    {
        var post = await IsExist(postId);
        return await ParsePost(post);
    }

    public async Task<PostModel> CreatePost(CreatePostModel model)
    {
        var post = new Post()
        {
            Title = model.Title,
            Description = model.Description,
            BlogId = model.BlogId
        };
        await _postRepository.CreatePost(post);
        return await ParsePost(post);
    }

    public async Task<PostModel> UpdatePost(Guid postId, CreatePostModel model)
    {
        var post = await IsExist(postId);
        post.Title = model.Title;
        post.Description = model.Description;
        post.UpdatedDate = DateTime.Now;
        await _postRepository.UpdatePost(post);
        return await ParsePost(post);
    }

    public async Task<string> DeletePost(Guid postId)
    {
        var post = await IsExist(postId);
        await _postRepository.DeletePost(post);
        return "Done :)";
    }
    
    public async Task<Like_Saved_Model?> Like(Guid postId)
    {
        var userId = _userProvider.UserId;
        var like =  await _postRepository.Like(postId,userId);
        if (like != null)return Parse_Like_Saved_Model(like);
        return null;
    }
    
    public async Task<List<SavedPost>> GetSavedPosts()
    {
        var userId = _userProvider.UserId;
        return await _postRepository.GetSavedPosts(userId);
    }

    public async Task<Like_Saved_Model?> SavePost(Guid postId)
    {
        var userId = _userProvider.UserId;
        var savePost = await _postRepository.SavePost(postId, userId);
        if (savePost != null) return Parse_Like_Saved_Model(savePost);
        return null;
    }
    
    /*
    public async Task<List<CommentModel>> GetComments()
    {
        var comments = await _postRepository.GetComments();
        return ParseListCommentModel(comments);
    }*/

    public async Task<List<CommentModel>> GetComments(Guid postId)
    {
        var comments = await _postRepository.GetComments(postId);
        return ParseListCommentModel(comments);
    }

    public async Task<CommentModel> CreateComment(CreateCommentModel model)
    {
        var comment = new Comment()
        {
            PostId = model.PostId,
            UserId = _userProvider.UserId,
            Message = model.Message
        };
        await _postRepository.CreateComment(comment);
        return ParseCommentModel(comment);
    }

    public async Task<CommentModel> UpdateComment(Guid commentId, CreateCommentModel model)
    {
        var comment = await IsExistComment(commentId);
        comment.Message = model.Message;
        await _postRepository.UpdateComment(comment);
        return ParseCommentModel(comment);
    }

    public async Task<string> DeleteComment(Guid commentId)
    {
        var comment = await IsExistComment(commentId);
        await _postRepository.DeleteComment(comment);
        return "All done :)";
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

    private List<Like_Saved_Model> Parse_Like_Saved_Model_List(List<SavedPost> list)
    {
        var listModel = new List<Like_Saved_Model>();
        foreach (var model in list)
        {
            listModel.Add(Parse_Like_Saved_Model(model));
        }
        return listModel;
    }

    private List<Like_Saved_Model> Parse_Like_Saved_Model_List(List<Like> list)
    {
        var listModel = new List<Like_Saved_Model>();
        foreach (var model in list)
        {
            listModel.Add(Parse_Like_Saved_Model(model));
        }
        return listModel;
    }

    private  async Task<PostModel> ParsePost(Post model)
    {
        Tuple<bool,int>like =  await GetLikes(model.PostId);
        var postModel = new PostModel()
        {
            PostId = model.PostId,
            Title = model.Title,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            IsLiked = like.Item1,
            LikeCount = like.Item2,
            Likes = Parse_Like_Saved_Model_List(model.Likes),
            BlogId = model.BlogId,
            SavedPosts = Parse_Like_Saved_Model_List(model.SavedPosts)
        };
        postModel.IsSaved = await IsSaved(model.PostId);

        return postModel;
    }

    public async Task<List<PostModel>> ParseListPostModel(List<Post> posts)
    {
        var postModels = new List<PostModel>();
        foreach (var post in posts)
        {
            postModels.Add(await ParsePost(post));
        }
        return postModels;
    }

    private CommentModel ParseCommentModel(Comment model)
    {
        return new CommentModel()
        {
            Id = model.Id,
            PostId = model.PostId,
            UserId = model.UserId,
            Message = model.Message,
            CreateDateTime = model.CreateDateTime
        };
    }

    private List<CommentModel> ParseListCommentModel(List<Comment> comments)
    {
        var commentModels = new List<CommentModel>();
        foreach (var comment in comments)
        {
            commentModels.Add(ParseCommentModel(comment));
        }
        return commentModels;
    }

    private async Task<Post> IsExist(Guid postId)
    {
        var post = await _postRepository.GetPostById(postId);
        if (post == null) throw new Exception("Not found");
        return post;
    }

    private async Task<Comment> IsExistComment(Guid commentId)
    {
        var comment = await _postRepository.GetCommentById(commentId);
        if (comment == null) throw new Exception("Not found");
        return comment;
    }

    private async Task<bool> IsSaved(Guid postId)
    {
        var userId = _userProvider.UserId;
        var savedPosts = await _postRepository.GetSavedPosts(userId);
        return savedPosts.FirstOrDefault(s => s.PostId == postId) != null;
        
    }
    
    private async Task<Tuple<bool, int>> GetLikes(Guid postId)
    {
        var userId = _userProvider.UserId;
        var likes = await _postRepository.GetLikes(postId);
        var like = likes.FirstOrDefault(l => l.UserId == userId);
        return new Tuple<bool, int>(like != null, likes.Count);
    }
}