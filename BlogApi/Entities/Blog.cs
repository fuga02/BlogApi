using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace BlogApi.Entities;

public class Blog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual List<Post> Posts { get; set; }
}