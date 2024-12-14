namespace WriteLens.Shared.Models;

public class DocumentType
{
    public Guid Id { get; set; }
    
    public string TypeName { get; set; }

    public string Desctiprion { get; set; }

    public DocumentTypeRuleset Ruleset { get; set; }
}