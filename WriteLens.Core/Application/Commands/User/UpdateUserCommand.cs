namespace WriteLens.Core.Application.Commands.User;

public record struct UpdateUserCommand(string? Email, string? Name);