namespace CMS.Domain.Entities;

public class CMSPage
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int MenuItemId { get; set; }
    public virtual MenuItem? MenuItem { get; set; }
    public int VisitorCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<PageContent> Contents { get; set; } = new List<PageContent>();
}
