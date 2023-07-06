namespace BlogApi.Models.BlogModels;

public class CommentModel
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreateDateTime { get; set; } 
}