namespace WriteLens.Shared.Models;

public class DocumentContentDocumentScore
{
    public decimal? TotalScore { 
        get {
            if (ReadabilityScore is null && AccessibilityScore is null) return null;
            if (ReadabilityScore is null) return AccessibilityScore;
            if (AccessibilityScore is null) return ReadabilityScore;
            return (ReadabilityScore + AccessibilityScore) / 2;
        }
    }
    public decimal? ReadabilityScore { get; set; }
    public decimal? AccessibilityScore { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}