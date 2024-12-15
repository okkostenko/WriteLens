using System.ComponentModel.DataAnnotations;

namespace WriteLens.Auth.WebAPI.DTOs.Requests;

/// <summary>
/// Request to register a user.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary> 
    /// <remarks>
    /// The email will be validated to match the email structure.
    /// </remarks> 
    /// <value></value>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// The password of the user.
    /// </summary>
    [Required]
    public string Password { get; set; }
}