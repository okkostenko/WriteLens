using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteLens.Core.Infrastructure.Data.PostgresDb.Entities;

[Table("documents")]
public class DocumentEntity
{
    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; set; }

    [Column("titile")]
    [Required]
    public string Title { get; set; }

    [Column("created_at")]
    [Required]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    [Required]
    public DateTimeOffset UpdatedAt { get; set; }

    [Column("archived_at")]
    public DateTimeOffset? ArchivedAt { get; set; }

    [ForeignKey("UserId")]
    public UserEntity User { get; set; }
}