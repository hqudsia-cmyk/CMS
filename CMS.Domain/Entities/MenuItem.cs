namespace CMS.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsVisible { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual MenuItem? Parent { get; set; }
    public virtual ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
    public virtual CMSPage? Page { get; set; }
}
