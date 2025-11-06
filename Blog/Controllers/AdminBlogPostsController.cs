using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Blog.Controllers;

public class AdminBlogPostsController : Controller
{
    private readonly ITag _tagRepository;
    private readonly IBlogPost _blogPostRepository;


    public AdminBlogPostsController(ITag tagRepository, IBlogPost blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var tags = await _tagRepository.GetAllAsync();
        
        var model = new AddBlogPost()
        {
            Tags = tags.Select(x => new SelectListItem()
            {
                Text = x.Name,//Ten gia tri
                Value = x.Id.ToString() // nhận được Id dưới dạng string
            })
        };
        return View(model);
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddBlogPost addBlogPost)
    {
        var blogPost = new BlogPost()
        {
            Heading = addBlogPost.Heading,
            PageTitle = addBlogPost.PageTitle,
            Content = addBlogPost.Content,
            ShortDescription = addBlogPost.ShortDescription,
            FeaturedImageUrl = addBlogPost.FeaturedImageUrl,
            UrlHandle = addBlogPost.UrlHandle,
            PublishedDate = addBlogPost.PublishedDate,
            Author = addBlogPost.Author,
            Visible = addBlogPost.Visible
        };
        //Tạo danh sách chứa các đối tượng Tag
        var tags = new List<Tag>();
        // Lay tag từ selected tags
        //Duyet qua từng Id đã chọn
        foreach (var selectedTagId in addBlogPost.SelectTags)
        {
            var selectTagIdGuid = Guid.Parse(selectedTagId);
            var tag = await _tagRepository.GetByIdAsync(selectTagIdGuid);
            if (tag != null)
            {
                tags.Add(tag);
            }
        }
        blogPost.Tags = tags;
        await _blogPostRepository.AddAsync(blogPost);
        
        return RedirectToAction("Add");
    }
    
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();
        return View(blogPosts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var blog = await _blogPostRepository.GetAsync(id);
        var tags = await _tagRepository.GetAllAsync();
        if (blog != null)
        {
            var blogModel = new EditBlogPost()
            {
                Id = blog.Id,
                Heading = blog.Heading,
                PageTitle = blog.PageTitle,
                Content = blog.Content,
                ShortDescription = blog.ShortDescription,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                UrlHandle = blog.UrlHandle,
                PublishedDate = blog.PublishedDate,
                Author = blog.Author,
                Visible = blog.Visible,
                Tags = tags.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                SelectTags = blog.Tags.Select(x => x.Id.ToString()).ToArray()
            
            };
            return View(blogModel);
        }
        
        return View("Error");
    }

    [HttpPost]
    [ActionName("Edit")]
    public async Task<IActionResult> Edit(EditBlogPost editBlogPost)
    {

        var blogPost = new BlogPost()
        {
            Id = editBlogPost.Id,
            Heading = editBlogPost.Heading,
            PageTitle = editBlogPost.PageTitle,
            Content = editBlogPost.Content,
            ShortDescription = editBlogPost.ShortDescription,
            FeaturedImageUrl = editBlogPost.FeaturedImageUrl,
            UrlHandle = editBlogPost.UrlHandle,
            PublishedDate = editBlogPost.PublishedDate,
            Author = editBlogPost.Author,
            Visible = editBlogPost.Visible,

        };
        // foreach (var selectedTagId in editBlogPost.SelectTags)
        // {
        //     var selectTagIdGuid = Guid.Parse(selectedTagId);
        //     var tag = await _tagRepository.GetByIdAsync(selectTagIdGuid);
        //     if (tag != null)
        //     {
        //         tags.Add(tag);
        //     }
        // }
        // blogPost.Tags = tags;
        // var updateBlog = await _blogPostRepository.UpdateAsync(blogPost);
        // if(updateBlog != null)
        // {
        //     return RedirectToAction("List", "AdminBlogPosts");
        // }
        var selectedTags = new List<Tag>();
        foreach (var selectedTag in editBlogPost.SelectTags)
        {
            if (Guid.TryParse(selectedTag, out var tag))
            {
                var foundTag = await _tagRepository.GetByIdAsync(tag);
                if (foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }
            }
        }
        blogPost.Tags = selectedTags;
        var editBlogPostResult = await _blogPostRepository.UpdateAsync(blogPost);
        if (editBlogPostResult != null)
        {
            return RedirectToAction("List");
        }

        return RedirectToAction("Edit", new { id = editBlogPost.Id });
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> Delete(EditBlogPost editBlogPost)
    {
        var blogPost = await _blogPostRepository.DeleteAsync(editBlogPost.Id);
        if(blogPost != null)
        {
            return RedirectToAction("List");
        }
        return RedirectToAction("Edit", new { id = editBlogPost.Id });
    }
}