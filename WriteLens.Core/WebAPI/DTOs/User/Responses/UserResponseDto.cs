namespace WriteLens.Core.WebAPI.DTOs.User.Responses;

/// <summary>
/// Represents a user.
/// </summary>
public class UserResponseDto
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The user creation time.
    /// </summary>
    public DateTimeOffset CreatedAt {get; set; }
}