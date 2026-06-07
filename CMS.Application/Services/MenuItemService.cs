using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.Application.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IRepository<MenuItem> _repository;

    public MenuItemService(IRepository<MenuItem> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetRootMenuItemsAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Where(m => m.ParentId == null).OrderBy(m => m.Order);
    }

    public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
    {
        menuItem.CreatedAt = DateTime.UtcNow;
        return await _repository.AddAsync(menuItem);
    }

    public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
    {
        return await _repository.UpdateAsync(menuItem);
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuHierarchyAsync()
    {
        var items = await _repository.GetAllAsync();
        var rootItems = items.Where(m => m.ParentId == null).OrderBy(m => m.Order);
        return rootItems;
    }
}
