using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.Services;

public class PageService : IPageService
{
    private readonly IRepository<CMSPage> _repository;

    public PageService(IRepository<CMSPage> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CMSPage>> GetAllPagesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<CMSPage?> GetPageByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<CMSPage?> GetPageBySlugAsync(string slug)
    {
        var pages = await _repository.GetAllAsync();
        return pages.FirstOrDefault(p => p.Slug == slug);
    }

    public async Task<CMSPage> CreatePageAsync(CMSPage page)
    {
        page.CreatedAt = DateTime.UtcNow;
        page.UpdatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(page);
    }

    public async Task<CMSPage> UpdatePageAsync(CMSPage page)
    {
        page.UpdatedAt = DateTime.UtcNow;
        return await _repository.UpdateAsync(page);
    }

    public async Task<bool> DeletePageAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task IncrementVisitorCountAsync(int pageId)
    {
        var page = await _repository.GetByIdAsync(pageId);
        if (page != null)
        {
            page.VisitorCount++;
            await _repository.UpdateAsync(page);
        }
    }
}
