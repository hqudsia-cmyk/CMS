using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.Services;

public class StyleConfigService : IStyleConfigService
{
    private readonly IRepository<StyleConfig> _repository;

    public StyleConfigService(IRepository<StyleConfig> repository)
    {
        _repository = repository;
    }

    public async Task<StyleConfig?> GetStyleByElementTypeAsync(string elementType)
    {
        var styles = await _repository.GetAllAsync();
        return styles.FirstOrDefault(s => s.ElementType == elementType);
    }

    public async Task<IEnumerable<StyleConfig>> GetAllStylesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<StyleConfig> CreateStyleAsync(StyleConfig style)
    {
        style.UpdatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(style);
    }

    public async Task<StyleConfig> UpdateStyleAsync(StyleConfig style)
    {
        style.UpdatedAt = DateTime.UtcNow;
        return await _repository.UpdateAsync(style);
    }

    public async Task<bool> DeleteStyleAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
