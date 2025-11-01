using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;
namespace Blog.Repository;

public class TagRepository:ITag
{
    private readonly BlogDbContext _blogDbContext;

    public TagRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _blogDbContext.Tags.ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Tag> AddTagAsync(Tag tag)
    {
        tag.Id = Guid.NewGuid();
        await _blogDbContext.Tags.AddAsync(tag);
        await  _blogDbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateAsync(Tag tag)
    {
       var existingTag =  await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
       if (existingTag != null)
       {
           existingTag.Name = tag.Name;
           existingTag.DisplayName = tag.DisplayName;
           await _blogDbContext.SaveChangesAsync();
           return existingTag;
       }
       return null!;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
       var existTag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
       if(existTag != null)
       {
           _blogDbContext.Tags.Remove(existTag);
           await _blogDbContext.SaveChangesAsync();
           return existTag;
       }
       return null;
    }
}