namespace WriteLens.Auth.Models.Commands;

public record struct LoginUserCommand(string Email, string Password);
