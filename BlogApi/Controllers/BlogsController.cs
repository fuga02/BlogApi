using BlogApi.Managers.BlogManagers;
using BlogApi.Models.BlogModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BlogsController : ControllerBase
{
    private readonly BlogManager _blogManager;

    public BlogsController(BlogManager blogManager)
    {
        _blogManager = blogManager;
    }

    public record Pagination(int Page, int Count);

    public record BlogFilter(
	    int Page,
	    int Count,
	    string? Title,
	    DateTime? FromDate,
	    DateTime? ToDate) : Pagination(Page, Count);

    [HttpGet]
    public async Task<IActionResult> GetBlogs([FromQuery]BlogFilter filter)
    {
	    var blogs = await _blogManager.GetBlogs();

	    if (filter.FromDate != null)
	    {
		    blogs = blogs.Where(b => b.CreatedDate > filter.FromDate).ToList();
	    }

	    if (filter.ToDate != null)
	    {
		    blogs = blogs.Where(b => b.CreatedDate < filter.ToDate).ToList();
	    }

	    if (filter.Title != null)
	    {
		    blogs = blogs.Where(b => b.Name.Contains(filter.Title)).ToList();
	    }

		// blogs = blogs.Skip((page - 1) * count).Take(count).ToList();
		//
		// Response.Headers.Add("totalpages", (blogs.Count / count).ToString());
		//
		// return Ok(blogs);

		// return Ok(blogs.ToPagedList(count));

		return Ok(blogs);
    }


    [HttpGet("blogId")]
    public async Task<IActionResult> GetBlogById(Guid blogId)
    {
        return Ok(await _blogManager.GetBlogById(blogId));
    }

    [HttpGet("userId")]
    public async Task<IActionResult> GetBlogByAuthor(Guid userId)
    {
        return Ok(await _blogManager.GetBlogByAuthor(userId));
    }

    [HttpGet("userName")]
    public async Task<IActionResult> GetBlogByAuthor(string userName)
    {
        return Ok(await _blogManager.GetBlogByAuthor(userName));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(CreateBlogModel model)
    {
        return Ok(await _blogManager.CreateBlog(model));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBlog(Guid blogId, CreateBlogModel model)
    {
        return Ok(await _blogManager.UpdateBlog(blogId, model));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        return Ok(await _blogManager.DeleteBlog(blogId));
    }
}