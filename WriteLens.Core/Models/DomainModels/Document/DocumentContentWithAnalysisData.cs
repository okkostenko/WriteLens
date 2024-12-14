using WriteLens.Shared.Models;


namespace WriteLens.Core.Models.DomainModels.Document;

public class DocumentContentWithAnalysisData
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DocumentType Type { get; set; }

    public List<DocumentContentSection>? Sections { get; set; }

    public List<DocumentContentFlag>? Flags { get; set; }

    public DocumentContentDocumentScore? Score { get; set; }
}