using System.ComponentModel.DataAnnotations;

namespace WriteLens.Accessibility.WebAPI.DTOs.Requests;

public class AccessibilityAnalysisRequestDto
{
    [Required]
    public Guid TaskId { get; set; }

    [Required]
    public Guid DocumentId { get; set; }
}