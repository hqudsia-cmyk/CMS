namespace CMS.Domain.Entities;

public class PageContent
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public int Order { get; set; }
    public string ContentType { get; set; } = string.Empty; // heading, text, image, link
    public string Content { get; set; } = string.Empty; // HTML content
    public string? CssClass { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual CMSPage? Page { get; set; }
}
 