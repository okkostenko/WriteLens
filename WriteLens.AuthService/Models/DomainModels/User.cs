using WriteLens.Auth.Models.Commands;

namespace WriteLens.Auth.Models.DomainModels.User;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt {get; set; }

    public User()
    {

    }

    public User (RegisterUserCommand registerData)
    {
        Id = Guid.NewGuid();
        Email = registerData.Email;
        PasswordHash = registerData.Password;
        Name = registerData.Name;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}