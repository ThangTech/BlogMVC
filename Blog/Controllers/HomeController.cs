using System.Diagnostics;
using Blog.Models;
using Blog.Models.ViewModels;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPost _blogPost;
        private readonly ITag _tag;

        public HomeController(ILogger<HomeController> logger, IBlogPost blogPost, ITag tag)
        {
            _logger = logger;
            _blogPost = blogPost;
            _tag = tag;
        }

        public async Task<IActionResult> Index()
        {
           var blogPosts =  await _blogPost.GetAllAsync();

           var tags = await _tag.GetAllAsync();
           var model = new HomeViewModel()
           {
               BlogPosts = blogPosts,
               Tags = tags
           };
           return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
