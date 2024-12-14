using WriteLens.Core.Application.Commands.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Models.DomainModels.Document;

public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CreatedAt {get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User.User? User { get; set; }

    public DocumentType? Type { get; set; }

    public DocumentContent? Content { get; set; }

    public List<DocumentContentFlag>? Flags { get; set; }

    public DocumentContentDocumentScore? Score { get; set; }

    public Document()
    {
        
    }
    public Document (CreateDocumentCommand createDocumentCommand)
    {
        Id = Guid.NewGuid();
        Title = createDocumentCommand.Title;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}