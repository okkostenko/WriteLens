using System.ComponentModel.DataAnnotations;
using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.Models.ApplicationModels;

public class MLTextAnalysisResultFlag
{
    [Required]
    public string OldText { get; set; }
    [Required]
    public string Suggestion { get; set; }
    public decimal Severity { get; set; }
    public DocumentContentFlagType FlagType { get; set; }
}