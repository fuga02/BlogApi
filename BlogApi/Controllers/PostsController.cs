using BlogApi.Managers.BlogManagers;
using BlogApi.Models.BlogModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/blogs/{blogId}/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly PostManager _postManager;

    public PostsController(PostManager postManager)
    {
        _postManager = postManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(Guid blogId)
    {
        return Ok(await _postManager.GetPosts(blogId));
    }

    /*[HttpGet("blogId")]
    public async Task<IActionResult> GetPosts(Guid blogId)
    {
        return Ok(await _postManager.GetPosts(blogId));
    }*/

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostById(Guid blogId, Guid postId)
    {
        return Ok(await _postManager.GetPostById(blogId,postId));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(Guid blogId, CreatePostModel model)
    {
        return Ok(await _postManager.CreatePost(blogId,model));
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid blogId,Guid postId, CreatePostModel model)
    {
        return Ok(await _postManager.UpdatePost(blogId,postId,model));
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(Guid blogId, Guid postId)
    {
        return Ok(await _postManager.DeletePost(blogId,postId));
    }
    

    [HttpPost("Like")]
    public async Task<IActionResult> Like(Guid blogId, Guid postId)
    {
        return Ok(await _postManager.Like(blogId,postId));
    }
    
    [HttpGet("get-saved-posts")]
    public async Task<IActionResult> GetSavedPosts(Guid blogId, Guid postId)
    {
        return Ok(await _postManager.GetSavedPosts(blogId,postId));
    }

    [HttpPost("save-post")]
    public async Task<IActionResult> SavePost(Guid blogId,Guid postId)
    {
        return Ok(await _postManager.SavePost(blogId,postId));
    }

    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetPostComments(Guid blogId, Guid postId)
    {
        return Ok(await _postManager.GetComments(blogId,postId));
    }

    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> CreateComment(Guid blogId, Guid postId, CreateCommentModel model)
    {
        return Ok(await _postManager.CreateComment(blogId,postId,model));
    }

    [HttpPut("{postId}/comments/{commentId}")]
    public async Task<IActionResult> UpdateComment(Guid blogId,Guid postId ,Guid commentId,CreateCommentModel model)
    {
        return Ok(await _postManager.UpdateComment(blogId,postId,commentId, model));
    }

    [HttpDelete("{postId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid blogId,Guid postId,Guid commentId)
    {
        return Ok(await _postManager.DeleteComment(blogId,postId,commentId));
    }
    
}