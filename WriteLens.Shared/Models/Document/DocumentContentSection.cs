namespace WriteLens.Shared.Models;

public class DocumentContentSection
{
    public Guid Id { get; set; }

    public decimal OrderIdx { get; set; }

    public string Content { get; set; }

    public string Hash { get; set; }

    public bool IsReadabilityAnalyzed { get; set; }
    
    public bool IsAccessibilityAnalyzed { get; set; }
}