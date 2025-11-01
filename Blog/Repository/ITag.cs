using Blog.Models.Domain;

namespace Blog.Repository;

public interface ITag
{
    Task<IEnumerable<Tag>> GetAllAsync();
    
    Task<Tag?> GetByIdAsync(Guid id);
    
    Task<Tag> AddTagAsync(Tag tag);
    
    Task<Tag> UpdateAsync(Tag tag);
    
    Task<Tag?> DeleteAsync(Guid id);
}