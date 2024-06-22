using API.web_h13p.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.web_h13p.Infatructure.DataAccess;

public class WebApiDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public WebApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("tbl_User");
        builder.Entity<IdentityRole>().ToTable("tbl_Role");
        builder.Entity<IdentityUserRole<string>>().ToTable("tbl_UserRole");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("tbl_RoleClaim");
        builder.Entity<IdentityUserClaim<string>>().ToTable("tbl_UserClaim");
        
    }
}