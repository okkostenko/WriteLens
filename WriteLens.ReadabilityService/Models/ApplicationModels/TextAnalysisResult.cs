namespace WriteLens.Readability.Models.ApplicationModels;

public class TextAnalysisResult
{
    public object Id { get; set; }
    public decimal Score { get; set; }
    public int TextLength { get; set;}
}