namespace BlogApi.Entities;

public class Post
{
    public Guid PostId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set;}

    public Guid BlogId { get; set; }
    public virtual Blog? Blog { get; set; }
    public virtual List<Like>? Likes { get; set; }
    public virtual List<SavedPost>?  SavedPosts { get; set; }
    public virtual List<Comment>? Comments { get; set; }

}