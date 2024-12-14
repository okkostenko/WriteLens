namespace WriteLens.Shared.Models;

public class DocumentContentScore
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public DocumentContentDocumentScore DocumentScore { get; set; }
    public List<DocumentContentSectionScore> SectionsScores { get; set; }
}