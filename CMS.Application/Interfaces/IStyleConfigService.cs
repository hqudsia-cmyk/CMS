using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IStyleConfigService
{
    Task<StyleConfig?> GetStyleByElementTypeAsync(string elementType);
    Task<IEnumerable<StyleConfig>> GetAllStylesAsync();
    Task<StyleConfig> CreateStyleAsync(StyleConfig style);
    Task<StyleConfig> UpdateStyleAsync(StyleConfig style);
    Task<bool> DeleteStyleAsync(int id);
}
