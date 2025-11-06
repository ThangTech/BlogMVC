using System.Net;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImagesRepository _imagesRepository;

    public ImagesController(IImagesRepository imagesRepository)
    {
        _imagesRepository = imagesRepository;
    }
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var imageUrl =  await _imagesRepository.UploadAsync(file);
        if (imageUrl == null)
        {
            return Problem("Error uploading image", null, (int)HttpStatusCode.InternalServerError);
        }
        return new JsonResult(new { link = imageUrl });
    }
}