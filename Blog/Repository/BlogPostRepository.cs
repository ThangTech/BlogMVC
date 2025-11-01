using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository;

public class BlogPostRepository:IBlogPost
{
    private readonly BlogDbContext _blogDbContext;

    public BlogPostRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _blogDbContext.BlogPosts.AddAsync(blogPost);
        await  _blogDbContext.SaveChangesAsync();
        return blogPost;
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlogPost = await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
        if (existingBlogPost != null)
        {
            existingBlogPost.Id = blogPost.Id;
            existingBlogPost.Heading = blogPost.Heading;
            existingBlogPost.PageTitle = blogPost.PageTitle;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.ShortDescription = blogPost.ShortDescription;
            existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlogPost.UrlHandle = blogPost.UrlHandle;
            existingBlogPost.PublishedDate = blogPost.PublishedDate;
            existingBlogPost.Author = blogPost.Author;
            existingBlogPost.Visible = blogPost.Visible;
            existingBlogPost.Tags = blogPost.Tags;
            // existingBlogPost.Tags.Clear();
            // foreach (var tag in blogPost.Tags)
            // {
            //     existingBlogPost.Tags.Add(tag);
            // }

            await _blogDbContext.SaveChangesAsync();
            return existingBlogPost;
        }

        return null;


    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
        var existingBlogPost = await _blogDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
        if (existingBlogPost != null)
        {
            _blogDbContext.Remove(existingBlogPost);
            await _blogDbContext.SaveChangesAsync();
            return existingBlogPost;
        }
        return null;
    }
}