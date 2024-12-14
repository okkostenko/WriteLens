using WriteLens.Core.Models.DomainModels.Document;

namespace WriteLens.Core.Models.DomainModels.User;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt {get; set; }
    public List<Document.Document>? Documents { get; set; }

    public User()
    {

    }
}