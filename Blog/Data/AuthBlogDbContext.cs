using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class AuthBlogDbContext : IdentityDbContext
{
    public AuthBlogDbContext(DbContextOptions<AuthBlogDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Seed Roles
        var adminRoleId = "72eb22eb-296f-4ee5-9cf4-5ac1952e5628";
        var superRoleId = "625e5b9b-7786-4757-847d-345508283d3a";
        var userRoleID = "625e5b9b-7786-4757-847d-345508283d5w";
        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            },
            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
                Id = superRoleId,
                ConcurrencyStamp = superRoleId
                
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "User",
                Id = userRoleID,
                ConcurrencyStamp = userRoleID
                
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);
        
        //Seed superadmin
        var superAdmin = "72eb22eb-296f-4ee5-9cf4-5ac1952e5624";
        var superAdminUser = new IdentityUser
        {
            UserName = "super@gmail.com",
            Email = "super@gmail.com",
            NormalizedEmail = "super@gmail.com".ToUpper(),
            NormalizedUserName = "super@gmail.com".ToUpper(),
            Id = superAdmin
        };
        superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "super123");
        builder.Entity<IdentityUser>().HasData(superAdminUser);
        
        //Add all roles to SuperAdminUser
        var superAdminRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = superAdmin
            },
            new IdentityUserRole<string>
            {
                RoleId = superRoleId,
                UserId = superAdmin
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleID,
                UserId = superAdmin
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
    }
}