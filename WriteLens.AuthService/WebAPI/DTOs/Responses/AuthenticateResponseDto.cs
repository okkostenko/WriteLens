namespace WriteLens.Auth.WebAPI.DTOs.Responses;

/// <summary>
/// Represent response body for authorization request.
/// </summary>
public class AuthenticateResponseDto
{
    /// <summary>
    /// JWT Token to be used as an authorization header.
    /// </summary>
    public string AuthToken { get; set; }

    public AuthenticateResponseDto(string token)
    {
        AuthToken = token;
    }
}