namespace BlogApi.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set;}

    public Guid BlogId { get; set; }
    public virtual Blog Blog { get; set; }

}