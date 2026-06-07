using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CMS.Infrastructure.Data;
using CMS.Domain.Entities;
using CMS.Application.Interfaces;
using CMS.Application.Services;
using CMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IPageContentService, PageContentService>();
builder.Services.AddScoped<IStyleConfigService, StyleConfigService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Razor Pages and Controllers (including controllers from CMS.API)
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(CMS.API.Controllers.PagesController).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Swagger — available in development only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapRazorPages();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CMS.Infrastructure.Data.ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CMS.Domain.Entities.ApplicationUser>>();
    await CMS.Infrastructure.Data.DbSeeder.SeedAsync(db, userManager);
}

app.Run();