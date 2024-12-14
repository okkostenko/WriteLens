namespace WriteLens.Accessibility.Models.ApplicationModels;

public class TextAnalysisResult
{
    public Guid Id { get; set; }
    public decimal Score { get; set; }
    public List<MLTextAnalysisResultFlag> Flags{ get; set;}
    public int TextLength { get; set;}
}