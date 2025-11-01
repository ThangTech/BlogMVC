using Blog.Models.Domain;

namespace Blog.Repository;

public interface IBlogPost
{
    Task<IEnumerable<BlogPost>> GetAllAsync();
    
    Task<BlogPost?> GetAsync(Guid id);
    
    Task<BlogPost> AddAsync(BlogPost blogPost);
    
    Task<BlogPost?> UpdateAsync(BlogPost blogPost);
    
    Task<BlogPost?> DeleteAsync(Guid id);
}