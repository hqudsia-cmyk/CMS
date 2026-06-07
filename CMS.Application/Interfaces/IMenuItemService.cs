using CMS.Domain.Entities;

namespace CMS.Application.Interfaces;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
    Task<MenuItem?> GetMenuItemByIdAsync(int id);
    Task<IEnumerable<MenuItem>> GetRootMenuItemsAsync();
    Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
    Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
    Task<bool> DeleteMenuItemAsync(int id);
    Task<IEnumerable<MenuItem>> GetMenuHierarchyAsync();
}
