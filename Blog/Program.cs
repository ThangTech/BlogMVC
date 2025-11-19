using Blog.Data;
using Blog.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("BlogConnectionString")));
builder.Services.AddDbContext<AuthBlogDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AuthBlogConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthBlogDbContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 6;
    opt.Password.RequiredUniqueChars = 1;
});
builder.Services.AddScoped<ITag, TagRepository>();
builder.Services.AddScoped<IBlogPost, BlogPostRepository>();
builder.Services.AddScoped<IImagesRepository, CloudiaryRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
