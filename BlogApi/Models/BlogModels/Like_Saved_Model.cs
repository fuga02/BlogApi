using BlogApi.Entities;

namespace BlogApi.Models.BlogModels;

public class Like_Saved_Model
{
    public Guid Id { get; set; } 
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}