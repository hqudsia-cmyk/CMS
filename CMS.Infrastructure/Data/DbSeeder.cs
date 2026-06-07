using CMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        await SeedAdminUserAsync(userManager);
        await SeedMenuItemsAndPagesAsync(db);
    }

    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@cms.com";

        if (await userManager.FindByEmailAsync(adminEmail) != null)
            return;

        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Admin User",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "Admin@1234");
    }

    private static async Task SeedMenuItemsAndPagesAsync(ApplicationDbContext db)
    {
        if (await db.MenuItems.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        // Menu items
        var home = new MenuItem { Title = "Home", Slug = "home", Order = 1, IsVisible = true, CreatedAt = now };
        var about = new MenuItem { Title = "About", Slug = "about", Order = 2, IsVisible = true, CreatedAt = now };
        var services = new MenuItem { Title = "Services", Slug = "services", Order = 3, IsVisible = true, CreatedAt = now };
        var contact = new MenuItem { Title = "Contact", Slug = "contact", Order = 4, IsVisible = true, CreatedAt = now };

        db.MenuItems.AddRange(home, about, services, contact);
        await db.SaveChangesAsync();

        // Pages with content
        var homePage = new CMSPage
        {
            Title = "Home",
            Slug = "home",
            MenuItemId = home.Id,
            CreatedAt = now,
            UpdatedAt = now,
            Contents = new List<PageContent>
            {
                new() { Order = 1, ContentType = "heading", Content = "Welcome to Our CMS", CreatedAt = now },
                new() { Order = 2, ContentType = "text", Content = "<p>This is a powerful and flexible content management system built with ASP.NET Core. Manage your pages, menus, and styles all from one place.</p>", CreatedAt = now },
                new() { Order = 3, ContentType = "text", Content = "<p>Use the admin panel to create and edit content, configure navigation, and customize the look and feel of your site.</p>", CreatedAt = now }
            }
        };

        var aboutPage = new CMSPage
        {
            Title = "About",
            Slug = "about",
            MenuItemId = about.Id,
            CreatedAt = now,
            UpdatedAt = now,
            Contents = new List<PageContent>
            {
                new() { Order = 1, ContentType = "heading", Content = "About Us", CreatedAt = now },
                new() { Order = 2, ContentType = "text", Content = "<p>We are a passionate team dedicated to building clean, maintainable, and scalable web solutions. Our CMS platform is designed to give content editors full control without touching code.</p>", CreatedAt = now },
                new() { Order = 3, ContentType = "text", Content = "<p>Founded in 2024, we believe that great software starts with a great foundation — clean architecture, solid principles, and a focus on user experience.</p>", CreatedAt = now }
            }
        };

        var servicesPage = new CMSPage
        {
            Title = "Services",
            Slug = "services",
            MenuItemId = services.Id,
            CreatedAt = now,
            UpdatedAt = now,
            Contents = new List<PageContent>
            {
                new() { Order = 1, ContentType = "heading", Content = "Our Services", CreatedAt = now },
                new() { Order = 2, ContentType = "text", Content = "<p>We offer a range of services to help you get the most out of your web presence:</p>", CreatedAt = now },
                new() { Order = 3, ContentType = "text", Content = "<ul><li><strong>Web Development</strong> — Custom ASP.NET Core applications tailored to your needs.</li><li><strong>CMS Setup</strong> — Full configuration and deployment of your content management system.</li><li><strong>Consulting</strong> — Architecture reviews and technology guidance.</li></ul>", CreatedAt = now }
            }
        };

        var contactPage = new CMSPage
        {
            Title = "Contact",
            Slug = "contact",
            MenuItemId = contact.Id,
            CreatedAt = now,
            UpdatedAt = now,
            Contents = new List<PageContent>
            {
                new() { Order = 1, ContentType = "heading", Content = "Contact Us", CreatedAt = now },
                new() { Order = 2, ContentType = "text", Content = "<p>Have a question or want to work together? Reach out to us and we'll get back to you as soon as possible.</p>", CreatedAt = now },
                new() { Order = 3, ContentType = "text", Content = "<p>Email: <a href=\"mailto:hello@cms.com\">hello@cms.com</a><br/>Phone: +1 (555) 000-1234<br/>Address: 123 Main Street, New York, NY 10001</p>", CreatedAt = now }
            }
        };

        db.Pages.AddRange(homePage, aboutPage, servicesPage, contactPage);
        await db.SaveChangesAsync();
    }
}
