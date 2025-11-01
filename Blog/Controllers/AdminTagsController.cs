using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

public class AdminTagsController : Controller
{
    private readonly ITag _tagRepository;


    public AdminTagsController(ITag tagRepository)
    {
        _tagRepository = tagRepository;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        // Tạo đối tượng Tag từ AddTagRequest
        var tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

        await _tagRepository.AddTagAsync(tag);

        return RedirectToAction("List", "AdminTags");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var tags = await _tagRepository.GetAllAsync();

        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
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
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };
        var existTag = await _tagRepository.UpdateAsync(tag);
        if (existTag != null)
        {
            return RedirectToAction("List", "AdminTags");
        }
        return RedirectToAction("Edit", "AdminTags");
       
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var existTag = await _tagRepository.DeleteAsync(editTagRequest.Id);
        if (existTag != null)
        {
            return RedirectToAction("List", "AdminTags");
        }    
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
        
    }
}