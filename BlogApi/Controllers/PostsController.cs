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
    public async Task<IActionResult> GetPosts()
    {
        return Ok(await _postManager.GetPosts());
    }

    /*[HttpGet("blogId")]
    public async Task<IActionResult> GetPosts(Guid blogId)
    {
        return Ok(await _postManager.GetPosts(blogId));
    }*/

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostById(Guid postId)
    {
        return Ok(await _postManager.GetPostById(postId));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostModel model)
    {
        return Ok(await _postManager.CreatePost(model));
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, CreatePostModel model)
    {
        return Ok(await _postManager.UpdatePost(postId,model));
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(Guid postId)
    {
        return Ok(await _postManager.DeletePost(postId));
    }
    

    [HttpPost("Like")]
    public async Task<IActionResult> Like(Guid postId)
    {
        return Ok(await _postManager.Like(postId));
    }
    
    [HttpGet("get-saved-posts")]
    public async Task<IActionResult> GetSavedPosts()
    {
        return Ok(await _postManager.GetSavedPosts());
    }

    [HttpPost("save-post")]
    public async Task<IActionResult> SavePost(Guid postId)
    {
        return Ok(await _postManager.SavePost(postId));
    }

    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetPostComments(Guid postId)
    {
        return Ok(await _postManager.GetComments(postId));
    }

    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> CreateComment(Guid postId, CreateCommentModel model)
    {
        return Ok(await _postManager.CreateComment(model));
    }

    [HttpPut("{postId}/comments/{commentId}")]
    public async Task<IActionResult> UpdateComment(Guid commentId,CreateCommentModel model)
    {
        return Ok(await _postManager.UpdateComment(commentId, model));
    }

    [HttpDelete("{postId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        return Ok(await _postManager.DeleteComment(commentId));
    }
    
}