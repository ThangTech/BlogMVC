using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Blog.Repository;

public class CloudiaryRepository : IImagesRepository
{
    private readonly IConfiguration _config;
    private readonly Account _account;

    public CloudiaryRepository(IConfiguration config)
    {
        _config = config;
        _account = new Account(
            _config.GetSection("Cloudiary")["CloudName"],
            _config.GetSection("Cloudiary")["ApiKey"],
            _config.GetSection("Cloudiary")["ApiSecret"]);
    }
    public async Task<string> UploadAsync(IFormFile file)
    {
        var client = new Cloudinary(_account);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            DisplayName = file.FileName
        };
        var uploadResult = await client.UploadAsync(uploadParams);
        if (uploadResult != null & uploadResult.StatusCode == HttpStatusCode.OK)
        {
            return uploadResult.SecureUri.ToString();
        }
        return null;
    }
}