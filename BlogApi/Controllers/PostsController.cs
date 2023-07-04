using BlogApi.Managers.BlogManagers;
using BlogApi.Models.BlogModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly PostManager _postManager;

    public PostsController(PostManager postManager)
    {
        _postManager = postManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        return Ok(await _postManager.GetPosts());
    }

    [HttpGet("blogId")]
    public async Task<IActionResult> GetPosts(Guid blogId)
    {
        return Ok(await _postManager.GetPosts(blogId));
    }

    [HttpGet("postId")]
    public async Task<IActionResult> GetPostById(Guid postId)
    {
        return Ok(await _postManager.GetPostById(postId));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostModel model)
    {
        return Ok(await _postManager.CreatePost(model));
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePost(Guid postId, CreatePostModel model)
    {
        return Ok(await _postManager.UpdatePost(postId,model));
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        return Ok(await _postManager.DeletePost(postId));
    }

    [HttpGet("GetLikes")]
    public async Task<IActionResult> GetLikes(Guid postId, Guid userId)
    {
        return Ok(await _postManager.GetLikes(postId, userId));
    }

    [HttpPost("Like")]
    public async Task<IActionResult> Like(Guid postId, Guid userId)
    {
        return Ok(await _postManager.Like(postId, userId));
    }

    [HttpGet("isSaved")]
    public async Task<IActionResult> IsSaved(Guid postId, Guid userId)
    {
        return Ok(await _postManager.IsSaved(postId, userId));
    }

    [HttpGet("GetSavedPosts")]
    public async Task<IActionResult> GetSavedPosts(Guid userId)
    {
        return Ok(await _postManager.GetSavedPosts(userId));
    }

    [HttpPost("savePost")]
    public async Task<IActionResult> SavePost(Guid postId, Guid userId)
    {
        return Ok(await _postManager.SavePost(postId, userId));
    }
}