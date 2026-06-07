using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IPageContentService
{
    Task<IEnumerable<PageContent>> GetPageContentsByPageIdAsync(int pageId);
    Task<PageContent?> GetContentByIdAsync(int id);
    Task<PageContent> CreateContentAsync(PageContent content);
    Task<PageContent> UpdateContentAsync(PageContent content);
    Task<bool> DeleteContentAsync(int id);
}
