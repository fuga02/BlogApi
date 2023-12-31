﻿using BlogApi.Managers.BlogManagers;
using BlogApi.Models.BlogModels;
using BlogApi.PaginationFilters;
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

    [HttpGet]
    public async Task<IActionResult> GetBlogs([FromQuery] BlogFilter filter)
    {
        return Ok(await _blogManager.GetBlogs());
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

        blogs = blogs.Skip((filter.Page - 1) * filter.Count).Take(filter.Count).ToList();

        Response.Headers.Add("totalPages", (blogs.Count / filter.Count).ToString());

        return Ok(blogs);
        
    }/*
    public async Task<IActionResult> GetBlogs()
    {
        return Ok(await _blogManager.GetBlogs());
    }*/

    [HttpGet("{blogId}")]
    public async Task<IActionResult> GetBlogById(Guid blogId)
    {
        return Ok(await _blogManager.GetBlogById(blogId));
    }
    

    [HttpPost]
    public async Task<IActionResult> CreateBlog(CreateBlogModel model)
    {
        return Ok(await _blogManager.CreateBlog(model));
    }

    [HttpPut("{blogId}")]
    public async Task<IActionResult> UpdateBlog(Guid blogId, CreateBlogModel model)
    {
        return Ok(await _blogManager.UpdateBlog(blogId, model));
    }

    [HttpDelete("{blogId}")]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        return Ok(await _blogManager.DeleteBlog(blogId));
    }
}