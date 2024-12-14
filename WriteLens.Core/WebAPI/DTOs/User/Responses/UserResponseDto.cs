namespace WriteLens.Core.WebAPI.DTOs.User.Responses;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt {get; set; }
}