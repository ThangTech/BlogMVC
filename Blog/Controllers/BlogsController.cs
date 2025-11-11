using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPost _blogPost;

    public BlogsController(IBlogPost blogPost)
    {
        _blogPost = blogPost;
    }

    public async Task<IActionResult> Index(string url)
    {
        var blogPost = await _blogPost.GetByUrlAsync(url);
        return View(blogPost);
    }
}