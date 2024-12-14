namespace WriteLens.Shared.Models;

public class DocumentContentSectionScore
{
    public Guid SectionId { get; set; }
    public decimal TotalScore { get; set; }
    public decimal ReadabilityScore { get; set; }
    public decimal AccessibilityScore { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}