namespace WriteLens.Auth.Models.Commands;

public record struct RegisterUserCommand(string Name, string Email, string Password);
