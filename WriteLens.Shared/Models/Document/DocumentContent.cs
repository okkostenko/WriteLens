namespace WriteLens.Shared.Models;

public class DocumentContent
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public List<DocumentContentSection>? Sections { get; set; }
}