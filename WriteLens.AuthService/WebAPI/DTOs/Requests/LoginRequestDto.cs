using System.ComponentModel.DataAnnotations;

namespace WriteLens.Auth.WebAPI.DTOs.Requests;

/// <summary>
/// Request to login a user.
/// </summary>
public class LoginRequestDto
{
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
    /// The passoword to the user's account.
    /// </summary>
    [Required]
    public string Password { get; set; }
}