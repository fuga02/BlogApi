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

    public async Task<List<PostModel>?> GetPosts(Guid blogId)
    {
        var posts = await _postRepository.GetPosts(blogId);
        return await ParseListPostModel(posts);
    }

    /*
    public async Task<List<PostModel>> GetPosts(Guid blogId)
    {
        var posts = await _dbContext.Posts.Where(p => p.BlogId ==  blogId).ToListAsync();
        return ParseList(posts);
    }*/

    public async Task<PostModel> GetPostById(Guid blogId, Guid postId)
    {
        var post = await IsExist(blogId, postId);
        return await ParsePost(post);
    }

    public async Task<PostModel> CreatePost(Guid blogId,CreatePostModel model)
    {
        var post = new Post()
        {
            Title = model.Title,
            Description = model.Description,
            BlogId = blogId
        };
        await _postRepository.CreatePost(blogId,post);
        return await ParsePost(post);
    }

    public async Task<PostModel> UpdatePost(Guid blogId,Guid postId, CreatePostModel model)
    {
        var post = await IsExist(blogId, postId);
        post.Title = model.Title;
        post.Description = model.Description;
        post.BlogId = blogId;
        post.UpdatedDate = DateTime.Now;
        await _postRepository.UpdatePost(blogId,post);
        return await ParsePost(post);
    }

    public async Task<string> DeletePost(Guid blogId, Guid postId)
    {
        var post = await IsExist(blogId,postId);
        await _postRepository.DeletePost(blogId,post);
        return "Done :)";
    }
    
    public async Task<Like_Saved_Model?> Like(Guid blogId,Guid postId)
    {
        var userId = _userProvider.UserId;
        var like =  await _postRepository.Like(blogId,postId,userId);
        if (like != null)return Parse_Like_Saved_Model(like);
        return null;
    }
    
    public async Task<List<SavedPost>> GetSavedPosts(Guid blogId,Guid postId)
    {
        var userId = _userProvider.UserId;
        return await _postRepository.GetSavedPosts(blogId,postId,userId);
    }

    public async Task<Like_Saved_Model?> SavePost(Guid blogId,Guid postId)
    {
        var userId = _userProvider.UserId;
        var savePost = await _postRepository.SavePost(blogId,postId, userId);
        if (savePost != null) return Parse_Like_Saved_Model(savePost);
        return null;
    }
    
    /*
    public async Task<List<CommentModel>> GetComments()
    {
        var comments = await _postRepository.GetComments();
        return ParseListCommentModel(comments);
    }*/

    public async Task<List<CommentModel>> GetComments(Guid blogId, Guid postId)
    {
        var comments = await _postRepository.GetComments(blogId,postId);
        return ParseListCommentModel(comments);
    }

    public async Task<CommentModel> CreateComment(Guid blogId,Guid postId,CreateCommentModel model)
    {
        var post = await IsExist(blogId, postId);
        
        var comment = new Comment()
        {
            PostId = postId,
            UserId = _userProvider.UserId,
            Message = model.Message
        };
        await _postRepository.CreateComment(blogId,postId,comment);
        return ParseCommentModel(comment);
    }

    public async Task<CommentModel> UpdateComment(Guid blogId,Guid postId,Guid commentId, CreateCommentModel model)
    {
        var comment = await IsExistComment(blogId, postId, commentId);
        comment.Message = model.Message;
        await _postRepository.UpdateComment(blogId,postId,comment);
        return ParseCommentModel(comment);
    }

    public async Task<string> DeleteComment(Guid blogId, Guid postId,Guid commentId)
    {
        var comment = await IsExistComment(blogId, postId, commentId);
        await _postRepository.DeleteComment(blogId,postId,comment);
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
        Tuple<bool,int>like =  await GetLikes(model.BlogId,model.PostId);
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
            SavedPosts = Parse_Like_Saved_Model_List(model.SavedPosts),
            Comments = ParseListCommentModel(model.Comments)
        };
        postModel.IsSaved = await IsSaved(model.BlogId, model.PostId);

        return postModel;
    }

    public async Task<List<PostModel>?> ParseListPostModel(List<Post>? posts)
    {
        var postModels = new List<PostModel>();
        if (posts != null)
        {
            foreach (var post in posts)
            {
                postModels.Add(await ParsePost(post));
            }
            return postModels;
        }
        return null;
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

    private async Task<Post> IsExist(Guid blogId,Guid postId)
    {
        var post = await _postRepository.GetPostById(blogId,postId);
        if (post == null) throw new Exception("Not found");
        return post;
    }

    private async Task<Comment> IsExistComment(Guid blogId, Guid postId,Guid commentId)
    {
        var comment = await _postRepository.GetCommentById(blogId,postId,commentId);
        if (comment == null) throw new Exception("Comment Not found");
        return comment;
    }

    private async Task<bool> IsSaved(Guid blogId,Guid postId)
    {
        var userId = _userProvider.UserId;
        var savedPosts = await _postRepository.GetSavedPosts(blogId, postId, userId);
        return savedPosts.FirstOrDefault(s => s.PostId == postId) != null;
        
    }
    
    private async Task<Tuple<bool, int>> GetLikes(Guid blogId,Guid postId)
    {
        var userId = _userProvider.UserId;
        var likes = await _postRepository.GetLikes(blogId,postId);
        var like = likes.FirstOrDefault(l => l.UserId == userId);
        return new Tuple<bool, int>(like != null, likes.Count);
    }
}