using System.ComponentModel.DataAnnotations;

namespace BlogApi.Entities;

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public string Message { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
}