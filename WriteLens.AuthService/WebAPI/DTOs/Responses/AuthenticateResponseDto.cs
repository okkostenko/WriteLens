namespace WriteLens.Auth.WebAPI.DTOs.Responses;

public class AuthenticateResponseDto
{
    public string AuthToken { get; set; }

    public AuthenticateResponseDto(string token)
    {
        AuthToken = token;
    }
}