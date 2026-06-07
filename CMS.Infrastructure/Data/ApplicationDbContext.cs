using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMS.Domain.Entities;

namespace CMS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<CMSPage> Pages { get; set; }
    public DbSet<PageContent> PageContents { get; set; }
    public DbSet<StyleConfig> StyleConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // MenuItem configuration
        builder.Entity<MenuItem>()
            .HasOne(m => m.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(m => m.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<MenuItem>()
            .HasOne(m => m.Page)
            .WithOne(p => p.MenuItem)
            .HasForeignKey<CMSPage>(p => p.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // Page configuration
        builder.Entity<CMSPage>()
            .HasMany(p => p.Contents)
            .WithOne(c => c.Page)
            .HasForeignKey(c => c.PageId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for performance
        builder.Entity<MenuItem>().HasIndex(m => m.Slug).IsUnique();
        builder.Entity<CMSPage>().HasIndex(p => p.Slug).IsUnique();
    }
}
