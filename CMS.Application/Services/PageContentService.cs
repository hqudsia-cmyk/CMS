using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.Services;

public class PageContentService : IPageContentService
{
    private readonly IRepository<PageContent> _repository;

    public PageContentService(IRepository<PageContent> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PageContent>> GetPageContentsByPageIdAsync(int pageId)
    {
        var contents = await _repository.GetAllAsync();
        return contents.Where(c => c.PageId == pageId).OrderBy(c => c.Order);
    }

    public async Task<PageContent?> GetContentByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<PageContent> CreateContentAsync(PageContent content)
    {
        content.CreatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(content);
    }

    public async Task<PageContent> UpdateContentAsync(PageContent content)
    {
        return await _repository.UpdateAsync(content);
    }

    public async Task<bool> DeleteContentAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
