using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.User.Requests;

public class UpdateUserRequestDto
{
    [EmailAddress]
    public string? Email { get; set; }
    public string Name { get; set; }
}