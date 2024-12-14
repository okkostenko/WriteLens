using System.ComponentModel.DataAnnotations;

namespace WriteLens.Core.WebAPI.DTOs.User.Requests;

/// <summary>
/// Request to update a user.
/// </summary> <summary>
/// 
/// </summary>
public class UpdateUserRequestDto
{
    /// <summary>
    /// The new email of the user.
    /// </summary> 
    /// <remarks>
    /// Email is optional parameters.
    /// If provided, will be validated to match the email structure.
    /// </remarks> 
    /// <value></value>
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// The new name of the user.
    /// </summary>
    public string Name { get; set; }
}