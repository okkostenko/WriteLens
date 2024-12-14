using WriteLens.Shared.Models;

namespace WriteLens.Readability.Models.DomainModels;

public class TaskModel
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = "Success";
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DocumentContentDocumentScore? Result { get; set; }
}