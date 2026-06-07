using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IPageService
{
    Task<IEnumerable<CMSPage>> GetAllPagesAsync();
    Task<CMSPage?> GetPageByIdAsync(int id);
    Task<CMSPage?> GetPageBySlugAsync(string slug);
    Task<CMSPage> CreatePageAsync(CMSPage page);
    Task<CMSPage> UpdatePageAsync(CMSPage page);
    Task<bool> DeletePageAsync(int id);
    Task IncrementVisitorCountAsync(int pageId);
}
