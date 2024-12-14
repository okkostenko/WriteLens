using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteLens.Auth.Infrastructure.Data.Entities;

[Table("users")]
public class UserEntity
{   
    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; }

    [Column("email")]
    [Required]
    public string Email { get; set; }

    [Column("password_hash")]
    [Required]
    public string PasswordHash { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("created_at")]
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    [Required]
    public DateTimeOffset UpdatedAt { get; set; }
}