using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

public class AdminTagsController : Controller
{
    private readonly BlogDbContext _context;

    public AdminTagsController(BlogDbContext blogDbContext)
    {
        this._context = blogDbContext;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ActionName("Add")]
    public IActionResult Add(AddTagRequest addTagRequest)
    {
        // Tạo đối tượng Tag từ AddTagRequest
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };
        _context.Tags.Add(tag);
        _context.SaveChanges();

        return RedirectToAction("List", "AdminTags");
    }

    [HttpGet]
    public IActionResult List()
    {
        var tags = _context.Tags.ToList();

        return View(tags);
    }

    [HttpGet]
    public IActionResult Edit(Guid id, EditTagRequest editTagRequest)
    {
        var tag = _context.Tags.FirstOrDefault(x => x.Id == id);
        if (tag == null)
        {
            return View(null);
        }

        var editTag = new EditTagRequest
        {
            Id = tag.Id,
            Name = tag.Name,
            DisplayName = tag.DisplayName
        };

        return View(editTag);
    }

    [HttpPost]
    [ActionName("Edit")]
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };
       var existTag =  _context.Tags.FirstOrDefault(x => x.Id == tag.Id);
       if (existTag != null)
       {
           existTag.Name = tag.Name;
           existTag.DisplayName = tag.DisplayName;
           _context.SaveChanges();
           return RedirectToAction("List", "AdminTags");
       }

       return RedirectToAction("Edit", "AdminTags");
       
    }

    [HttpPost]
    [ActionName("Delete")]
    public IActionResult Delete(EditTagRequest editTagRequest)
    {
        var tag = _context.Tags.FirstOrDefault(x => x.Id == editTagRequest.Id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            
            return RedirectToAction("List", "AdminTags");
        }
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
        
    }
}