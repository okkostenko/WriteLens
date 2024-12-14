namespace WriteLens.Shared.Models;

public class DocumentContentLength
{
    public int DocumentLength { get; set; }
    public Dictionary<Guid, int> SectionsLengthes { get; set; }
}