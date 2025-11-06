namespace Blog.Repository;

public interface IImagesRepository
{
    Task<string> UploadAsync(IFormFile file);
}