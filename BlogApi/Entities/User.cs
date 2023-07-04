namespace BlogApi.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public required string Username { get; set; }
    public string PasswordHash { get; set; }

    public virtual List<Blog> Blogs { get; set; }
    public virtual List<Like> Likes { get; set; }
    public virtual List<SavedPost> SavedPosts { get; set; }
}