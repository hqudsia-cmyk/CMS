namespace CMS.Domain.Entities;

public class StyleConfig
{
    public int Id { get; set; }
    public string ElementType { get; set; } = string.Empty; // heading, text, image, link
    public string CssStyles { get; set; } = string.Empty; // CSS rules
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
