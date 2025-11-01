using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Models.ViewModels;

public class EditBlogPost
{
    public Guid Id { get; set; }
    public string Heading { get; set; }
    public string PageTitle { get; set; }
    public string Content { get; set; }
    public string ShortDescription { get; set; }
    public string FeaturedImageUrl { get; set; }
    public string UrlHandle { get; set; }
    public DateTime PublishedDate { get; set; }
    public string Author { get; set; }
    public bool Visible { get; set; }
    //Muốn lấy các tag dã tồn tại trong db để hiển thị trong dropdown
    //Hiển thị tag
    public IEnumerable<SelectListItem> Tags { get; set; }
    
    //Lấy các tag mà user chọn
    public string[] SelectTags { get; set; } = Array.Empty<string>();
}